using System;
using System.Data;
using System.Data.SqlClient;
using Commons.Extensions;
using DataAccessLibrary.Core;
using DataAccessLibrary.DataTransferObjects;
using DataAccessLibrary.Extensions;
using DataAccessLibrary.Internal.SQL;
using DataAccessLibrary.Internal.SQL.Enums;
using System.Configuration;
using DataAccessLibrary.Internal.ParameterNames;

// ReSharper disable CoVariantArrayConversion

namespace DataAccessLibrary.DataAccessors
{
    public abstract class SqlDatabaseAccessAbstract<T> : DatabaseAccessAbstract<T, int> where T : SqlDataTransferObject
    {


        private static readonly string ConnectionString = ConfigurationManager.ConnectionStrings.GetConnectionString();

        private const string cShouldHaveOnlyOneRow = "Should have only one row!";


        public override T[] GetAll()
        {
            SqlCallParameters parameters = CreateDefaultParameters(0, Actions.GET_ALL);
            DataTable dataTable = GetDataTable(parameters);

            return SetDataTransferObjectsFromDataTable(dataTable);
        }

        public override T GetById(int id)
        {
            SqlCallParameters parameters = CreateDefaultParameters(1, Actions.GET_BY_ID, id);
            DataTable dataTable = GetDataTable(parameters);

            if (dataTable.Rows.Count > 1)
                throw new Exception(cShouldHaveOnlyOneRow);

            return CreateFromRow(dataTable.Rows[0]);
        }

        public override int Insert(T item)
        {
            SqlCallParameters parameters = CreateParameters(item, Actions.INSERT);

            return ExecuteStoredProcedure(parameters);
        }

        public override T[] InsertMany(T[] items)
        {
            if (items.IsNullOrEmpty())
                return Array.Empty<T>();

            if (items.Length > 20000)
            {
                //stagger

                T[] arr = new T[20000];
                DataTable res = new DataTable();
                foreach (ColumnDefinition cd in items[0].ColumnNames())
                {
                    res.Columns.Add(new DataColumn(cd.Name, cd.DataType.ToUnderlyingType()));
                }

                for (int i = 0; i < items.Length; i++)
                {
                    arr[i % 20000] = items[i];

                    if (i % 20000 == 20000 - 1)
                    {
                        DataTable dt = arr.ToDataTable();

                        DataTable inserted = ExecuteNonQuery(dt, Actions.INSERT_MANY);

                        foreach (DataRow row in inserted.Rows)
                        {

                            res.Rows.Add(row.ItemArray);
                        }
                    }
                }

                return SetDataTransferObjectsFromDataTable(res);
            }
            else
            {

                DataTable dt = items.ToDataTable();

                DataTable inserted = ExecuteNonQuery(dt, Actions.INSERT_MANY);

                return SetDataTransferObjectsFromDataTable(inserted);
            }
        }

        public override void UpdateMany(T[] items, int id)
        {
            if (items.IsNullOrEmpty())
                return;
            DataTable dt = items.ToDataTable();

            ExecuteNonQuery(dt, Actions.UPDATE_MANY);
        }

        public override void DeleteItem(T item)
        {
            SqlCallParameters parameters = CreateParameters(item, Actions.DELETE_ITEM);
            ExecuteStoredProcedure(parameters);
        }

        public override void DeleteById(int id)
        {
            SqlCallParameters parameters = CreateDefaultParameters(2, Actions.DELETE_BY_ID, id);
            ExecuteStoredProcedure(parameters);
        }

        public override int Update(T item)
        {
            SqlCallParameters parameters = CreateParameters(item, Actions.UPDATE);
            return ExecuteStoredProcedure(parameters);
        }

        public override int Upsert(T item)
        {
            SqlCallParameters parameters = CreateParameters(item, Actions.UPSERT);
            return ExecuteStoredProcedure(parameters);
        }

        private static int ExecuteStoredProcedure(SqlCallParameters sqlCallParameters)
        {
            object res;
            try
            {

                using SqlConnection con = new(ConnectionString);
                using SqlCommand cmd = new(sqlCallParameters.StoredProcedure, con)
                {
                    CommandType = CommandType.StoredProcedure
                };

                sqlCallParameters.FillParameters(cmd.Parameters);

                con.Open();

                cmd.ExecuteNonQuery();

                //this goes with the assumption that the ID is output or inputoutput value.
                res = cmd.Parameters[ParameterNames.SQL.cId].Value;

                con.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            return res.TryParse(out int i) ? i : throw new Exception();
        }

        private T[] SetDataTransferObjectsFromDataTable(DataTable dataTable)
        {
            T[] res = new T[dataTable.Rows.Count];

            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                res[i] = CreateFromRow(dataTable.Rows[i]);
            }

            return res;
        }

        internal SqlCallParameters CreateDefaultParameters(int parametersCount, Actions action) => new(parametersCount, Table, action);

        private SqlCallParameters CreateDefaultParameters(int parametersCount, Actions action, int id)
        {
            SqlCallParameters parameters = CreateDefaultParameters(parametersCount, action);
            parameters.AddParameter(ParameterNames.SQL.cId, id, DataType.Int, Direction.InputOutput);
            return parameters;
        }

        protected abstract T CreateFromRow(DataRow row);

        private static DataTable GetDataTable(SqlCallParameters parameters)
        {
            try
            {
                using SqlConnection con = new(ConnectionString);
                using SqlCommand cmd = new(parameters.StoredProcedure)
                {
                    Connection = con,
                    CommandType = CommandType.StoredProcedure
                };
                parameters.FillParameters(cmd.Parameters);

                DataTable dt = new();

                using SqlDataAdapter adapter = new(cmd);
                adapter.Fill(dt);

                return dt;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        private SqlCallParameters CreateParameters(T item, Actions action)
        {
            return item.CreateParameters(CreateDefaultParameters(item.ParametersCount, action));
        }

        private DataTable ExecuteNonQuery(DataTable dataTablet, Actions action)
        {
            try
            {
                using SqlConnection con = new(ConnectionString);
                using SqlCommand cmd = new(Table.CreateStoredProcedureName(action), con)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.Parameters.AddWithValue(ParameterNames.SQL.cDataTable, dataTablet);

                DataSet result = new();

                using SqlDataAdapter adapter = new(cmd);
                adapter.Fill(result);


                return result.Tables[0];
            }
            catch (Exception e)
            {
                //TODO: do some sort of logging system, online db wouldnt be a bad idea too
                throw;
            }
        }
    }
}