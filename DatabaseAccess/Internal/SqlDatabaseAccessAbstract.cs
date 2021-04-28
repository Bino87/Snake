using System;
using System.Data;
using System.Data.SqlClient;
using Commons.Extensions;
using DataAccessLibrary.Core;
using DataAccessLibrary.DataTransferObjects;
using DataAccessLibrary.Extensions;
using DataAccessLibrary.Internal.SQL;
using DataAccessLibrary.Internal.SQL.Enums;
using DataAccessLibrary.Internal.SQL.ParameterNames;
using System.Configuration;
// ReSharper disable CoVariantArrayConversion

namespace DataAccessLibrary.Internal
{
    public abstract class SqlDatabaseAccessAbstract<T> : DatabaseAccessAbstract<T> where T : SqlDataTransferObject
    {
        private const string cDataTable = "DATA";

        private static readonly string ConnectionString = ConfigurationManager.ConnectionStrings.GetConnectionString();

        private const string cShouldHaveOnlyOneRow = "Should have only one row!";

        protected abstract Table Table { get; }
        public override T[] GetAll()
        {
            SqlCallParameters parameters = CreateDefaultParameters(1, Actions.GET_ALL);
            DataTable dataTable = GetDataTable(parameters);

            return  SetDataTransferObjectsFromDataTable(dataTable);
        }

        public override T GetById(int id)
        {
            SqlCallParameters parameters = CreateDefaultParameters(2, Actions.GET_BY_ID, id);
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

        public override int InsertMany(T[] items)
        {
            if (items.IsNullOrEmpty())
                return -1;
            DataTable dt = items.ToDataTable();

            return ExecuteNonQuery(dt, Actions.INSERT_MANY);
        }

        public override int UpdateMany(T[] items)
        {
            if (items.IsNullOrEmpty())
                return -1;
            DataTable dt = items.ToDataTable();

            return ExecuteNonQuery(dt, Actions.UPDATE_MANY);
        }

        public override int UpsertMany(T[] items)
        {
            if (items.IsNullOrEmpty())
                return -1;
            DataTable dt = items.ToDataTable();

            return ExecuteNonQuery(dt, Actions.UPSERT_MANY);
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
                res = cmd.Parameters[ParameterNames.cId].Value;

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

        private SqlCallParameters CreateDefaultParameters(int parametersCount, Actions action) => new(parametersCount, Table, action);

        private SqlCallParameters CreateDefaultParameters(int parametersCount, Actions action, int id)
        {
            SqlCallParameters parameters = CreateDefaultParameters(parametersCount, action);
            parameters.AddParameter(ParameterNames.cId, id, DataType.Int, Direction.InputOutput);
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

        private int ExecuteNonQuery(DataTable dt, Actions action)
        {
            int res;
            try
            {
                using SqlConnection con = new(ConnectionString);
                using SqlCommand cmd = new(Table.CreateStoredProcedureName(action), con)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.Parameters.AddWithValue(cDataTable, dt);

                con.Open();
                res = cmd.ExecuteNonQuery();
                con.Close();
            }
            catch (Exception e)
            {
                //TODO: do some sort of logging system, online db wouldnt be a bad idea too
                throw;
            }

            return res;
        }
    }
}