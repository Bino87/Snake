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
    public class SqlDatabaseAccessAbstract : DatabaseAccessAbstract<SqlDataTransferObject>
    {
        private const string CConnectionString = @"Data Source=(localdb)\MSSQLLocalDB;Integrated Security=True;Persist Security Info=False;Pooling=False;MultipleActiveResultSets=False;Connect Timeout=60;Encrypt=False;TrustServerCertificate=False";

        internal Table Table { get; }

        public override async Task<SqlDataTransferObject[]> GetAllAsync()
        {
            SqlCallParameters parameters = CreateDefaultParameters(1, Actions.SELECT_ALL);
            DataTable dt = await GetDataTableAsync(parameters);
            SqlDataTransferObject[] res = SetDataTransferObjectsFromDataTable(dt);

            return res;
        }

        public override SqlDataTransferObject[] GetAll()
        {
            SqlCallParameters parameters = CreateDefaultParameters(1, Actions.SELECT_ALL);
            DataTable dataTable = GetDataTable(parameters);

            SqlDataTransferObject[] res = SetDataTransferObjectsFromDataTable(dataTable);

            return res;
        }

        public override SqlDataTransferObject GetById(int id)
        {
            SqlCallParameters parameters = CreateDefaultParameters(2, Actions.SELECT_BY_ID, id);
            DataTable dataTable = GetDataTable(parameters);

            if (dataTable.Rows.Count > 1)
                throw new Exception("should have only one row!");

            return CreateFromRow(dataTable.Rows[0]);
        }

        public override async Task<SqlDataTransferObject> GetByIdAsync(int id)
        {
            SqlCallParameters parameters = CreateDefaultParameters(2, Actions.SELECT_BY_ID, id);
            DataTable dataTable = await GetDataTableAsync(parameters);
            if (dataTable.Rows.Count > 1)
                throw new Exception("should have only one row!");

            return CreateFromRow(dataTable.Rows[0]);
        }

        public override int Insert(SqlDataTransferObject item)
        {
            SqlCallParameters parameters = item.CreateParameters(CreateDefaultParameters(item.ParametersCount, Actions.INSERT));

            return ExecuteStoredProcedure(parameters);
        }

        public override int InsertMany(SqlDataTransferObject[] items)
        {
            int res = 0;
            if (items.IsEmpty())
                return res;
            DataTable dt = items.ToDataTable();


            using SqlConnection con = new(CConnectionString);
            using SqlCommand cmd = new("INSERT_MANY", con)
            {
                CommandType = CommandType.StoredProcedure
            };
            cmd.Parameters.AddWithValue("DATA_TABLE", dt);
            con.Open();
            res = cmd.ExecuteNonQuery();
            con.Close();

            return res;

        }

        public override async Task<int> InsertAsync(SqlDataTransferObject item)
        {
            SqlCallParameters parameters = item.CreateParameters(CreateDefaultParameters(item.ParametersCount, Actions.INSERT));
            return await ExecuteStoredProcedureAsync(parameters);
        }

        public override void DeleteItem(SqlDataTransferObject item)
        {
            SqlCallParameters parameters = item.CreateParameters(CreateDefaultParameters(item.ParametersCount, Actions.DELETE_ITEM));
            ExecuteStoredProcedure(parameters);
        }

        public override async Task DeleteItemAsync(SqlDataTransferObject item)
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

        public override int Update(SqlDataTransferObject item)
        {
            SqlCallParameters parameters = item.CreateParameters(CreateDefaultParameters(item.ParametersCount, Actions.UPDATE));
            return ExecuteStoredProcedure(parameters);
        }

        public override async Task<int> UpdateAsync(SqlDataTransferObject item)
        {
            SqlCallParameters parameters = item.CreateParameters(CreateDefaultParameters(item.ParametersCount, Actions.UPDATE));
            return await ExecuteStoredProcedureAsync(parameters);
        }

        public override int Upsert(SqlDataTransferObject item)
        {
            SqlCallParameters parameters = item.CreateParameters(CreateDefaultParameters(item.ParametersCount, Actions.UPSERT));
            return ExecuteStoredProcedure(parameters);
        }

        public override async Task<int> UpsertAsync(SqlDataTransferObject item)
        {
            SqlCallParameters parameters = item.CreateParameters(CreateDefaultParameters(item.ParametersCount, Actions.UPSERT));
            return await ExecuteStoredProcedureAsync(parameters);
        }

        private static int ExecuteStoredProcedure(SqlCallParameters sqlCallParameters)
        {
            object res;
            try
            {

                using SqlConnection con = new(CConnectionString);
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
                await using SqlConnection con = new(CConnectionString);
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

        private SqlDataTransferObject[] SetDataTransferObjectsFromDataTable(DataTable dataTable)
        {
            SqlDataTransferObject[] res = new SqlDataTransferObject[dataTable.Rows.Count];

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

        protected virtual SqlDataTransferObject CreateFromRow(DataRow dataTableRow)
        {
            return new(dataTableRow);
        }

        private static async Task<DataTable> GetDataTableAsync(SqlCallParameters parameters)
        {
            try
            {
                await using SqlConnection con = new(CConnectionString);
                await using SqlCommand cmd = new(parameters.StoredProcedure)
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

        private static DataTable GetDataTable(SqlCallParameters parameters)
        {

            try
            {
                using SqlConnection con = new(CConnectionString);
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