using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Commons.Extensions;
using DataAccessLibrary.Core;
using DataAccessLibrary.DataTransferObjects;
using DataAccessLibrary.Extensions;
using DataAccessLibrary.Internal.SQL;
using DataAccessLibrary.Internal.SQL.Enums;
using DataAccessLibrary.Internal.SQL.ParameterNames;

namespace DataAccessLibrary.Internal
{
    public abstract class SqlDatabaseAccessAbstract<T> : DatabaseAccessAbstract<T> where T : SqlDataTransferObject
    {
        private const string cDataTable = "DATA_TABLE";

        private const string cConnectionString = @"Data Source=(localdb)\MSSQLLocalDB;Integrated Security=True;Persist Security Info=False;Pooling=False;MultipleActiveResultSets=False;Connect Timeout=60;Encrypt=False;TrustServerCertificate=False";
        private const string cConnectionstrink = @"Data Source=(localdb)\MSSQLLocalDB;Integrated Security=True;Persist Security Info=False;Pooling=False;MultipleActiveResultSets=False;Connect Timeout=60;Encrypt=False;TrustServerCertificate=False";
        private const string cShouldHaveOnlyOneRow = "Should have only one row!";

        protected abstract Table Table { get; }

        public override async Task<T[]> GetAllAsync()
        {
            SqlCallParameters parameters = CreateDefaultParameters(1, Actions.SELECT_ALL);
            DataTable dt = await Task.Run(() => GetDataTable(parameters));
            T[] res = SetDataTransferObjectsFromDataTable(dt);

            return res;
        }

        public override T[] GetAll()
        {
            SqlCallParameters parameters = CreateDefaultParameters(1, Actions.SELECT_ALL);
            DataTable dataTable = GetDataTable(parameters);

            T[] res = SetDataTransferObjectsFromDataTable(dataTable);

            return res;
        }

        public override T GetById(int id)
        {
            SqlCallParameters parameters = CreateDefaultParameters(2, Actions.SELECT_BY_ID, id);
            DataTable dataTable = GetDataTable(parameters);

            if (dataTable.Rows.Count > 1)
                throw new Exception(cShouldHaveOnlyOneRow);

            return CreateFromRow(dataTable.Rows[0]);
        }

        public override async Task<T> GetByIdAsync(int id)
        {
            SqlCallParameters parameters = CreateDefaultParameters(2, Actions.SELECT_BY_ID, id);
            DataTable dt = await Task.Run(() => GetDataTable(parameters));
            if (dt.Rows.Count != 1)
                throw new Exception(cShouldHaveOnlyOneRow);

            return CreateFromRow(dt.Rows[0]);
        }

        public override int Insert(T item)
        {
            SqlCallParameters parameters = item.CreateParameters(CreateDefaultParameters(item.ParametersCount, Actions.INSERT));

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

        public override async Task<int> InsertManyAsync(T[] items)
        {
            if (items.IsNullOrEmpty())
                return -1;
            DataTable dt = items.ToDataTable();

            return await ExecuteNonQueryAsync(dt, Actions.INSERT_MANY); ;
        }

        public override async Task<int> UpsertManyAsync(T[] items)
        {
            if (items.IsNullOrEmpty())
                return -1;
            DataTable dt = items.ToDataTable();

            return await ExecuteNonQueryAsync(dt, Actions.UPSERT_MANY); ;
        }

        public override async Task<int> UpdateManyAsync(T[] items)
        {
            if (items.IsNullOrEmpty())
                return -1;
            DataTable dt = items.ToDataTable();

            return await ExecuteNonQueryAsync(dt, Actions.UPDATE_MANY); ;
        }

        private async Task<int> ExecuteNonQueryAsync(DataTable dt, Actions actions)
        {
            int res;
            try
            {
                await using SqlConnection con = new(cConnectionString);
                await using SqlCommand cmd = new(Table.CreateStoredProcedureName(actions), con)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.Parameters.AddWithValue(cDataTable, dt);

                await con.OpenAsync();
                res = await cmd.ExecuteNonQueryAsync();
                con.Close();
            }
            catch (Exception)
            {
                throw;
            }

            return res;
        }

        private int ExecuteNonQuery(DataTable dt, Actions action)
        {
            int res;
            try
            {
                using SqlConnection con = new(cConnectionString);
                using SqlCommand cmd = new(Table.CreateStoredProcedureName(action), con)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.Parameters.AddWithValue(cDataTable, dt);

                con.Open();
                res = cmd.ExecuteNonQuery();
                con.Close();
            }
            catch (Exception)
            {
                throw;
            }

            return res;
        }

        public override async Task<int> InsertAsync(T item)
        {
            SqlCallParameters parameters = item.CreateParameters(CreateDefaultParameters(item.ParametersCount, Actions.INSERT));
            return await ExecuteStoredProcedureAsync(parameters);
        }

        public override void DeleteItem(T item)
        {
            SqlCallParameters parameters = item.CreateParameters(CreateDefaultParameters(item.ParametersCount, Actions.DELETE_ITEM));
            ExecuteStoredProcedure(parameters);
        }

        public override async Task DeleteItemAsync(T item)
        {
            SqlCallParameters parameters = item.CreateParameters(CreateDefaultParameters(item.ParametersCount, Actions.DELETE_ITEM));
            await ExecuteStoredProcedureAsync(parameters);
        }

        public override void DeleteById(int id)
        {
            SqlCallParameters parameters = CreateDefaultParameters(2, Actions.DELETE_BY_ID, id);
            ExecuteStoredProcedure(parameters);
        }

        public override async Task DeleteByIdAsync(int id)
        {
            SqlCallParameters parameters = CreateDefaultParameters(2, Actions.DELETE_BY_ID, id);
            await ExecuteStoredProcedureAsync(parameters);
        }

        public override int Update(T item)
        {
            SqlCallParameters parameters = item.CreateParameters(CreateDefaultParameters(item.ParametersCount, Actions.UPDATE));
            return ExecuteStoredProcedure(parameters);
        }

        public override async Task<int> UpdateAsync(T item)
        {
            SqlCallParameters parameters = item.CreateParameters(CreateDefaultParameters(item.ParametersCount, Actions.UPDATE));
            return await ExecuteStoredProcedureAsync(parameters);
        }

        public override int Upsert(T item)
        {
            SqlCallParameters parameters = item.CreateParameters(CreateDefaultParameters(item.ParametersCount, Actions.UPSERT));
            return ExecuteStoredProcedure(parameters);
        }

        public override async Task<int> UpsertAsync(T item)
        {
            SqlCallParameters parameters = item.CreateParameters(CreateDefaultParameters(item.ParametersCount, Actions.UPSERT));
            return await ExecuteStoredProcedureAsync(parameters);
        }

        private static int ExecuteStoredProcedure(SqlCallParameters sqlCallParameters)
        {
            object res;
            try
            {

                using SqlConnection con = new(cConnectionString);
                using SqlCommand cmd = new(sqlCallParameters.StoredProcedure, con)
                {
                    CommandType = CommandType.StoredProcedure
                };

                sqlCallParameters.FillParameters(cmd.Parameters);

                con.Open();

                res = cmd.ExecuteScalar();

                con.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            return res.TryParse(out int i) ? i : throw new Exception();
        }

        private static async Task<int> ExecuteStoredProcedureAsync(SqlCallParameters parameters)
        {
            object res;
            try
            {
                await using SqlConnection con = new(cConnectionString);
                await using SqlCommand cmd = new(parameters.StoredProcedure, con)
                {
                    CommandType = CommandType.StoredProcedure
                };

                parameters.FillParameters(cmd.Parameters);

                await con.OpenAsync();

                res = await cmd.ExecuteScalarAsync();

                await con.CloseAsync();
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

        protected abstract T CreateFromRow(DataRow dataTableRow);

        private static DataTable GetDataTable(SqlCallParameters parameters)
        {
            try
            {
                using SqlConnection con = new(cConnectionString);
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
    }
}