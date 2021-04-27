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
        private const string cConnectionString = @"Data Source=(localdb)\MSSQLLocalDB;Integrated Security=True;Persist Security Info=False;Pooling=False;MultipleActiveResultSets=False;Connect Timeout=60;Encrypt=False;TrustServerCertificate=False";


        public override async Task<T[]> GetAllAsync()
        {
            SqlCallParameters parameters = CreateDefaultParameters(1, Actions.SelectAll);
            DataTable dt = await GetDataTableAsync(parameters);
            T[] res = SetDataTransferObjectsFromDataTable(dt);

            return res;
        }

        public override T[] GetAll()
        {
            SqlCallParameters parameters = CreateDefaultParameters(1, Actions.SelectAll);
            DataTable dataTable = GetDataTable(parameters);

            T[] res = SetDataTransferObjectsFromDataTable(dataTable);

            return res;
        }

        public override T GetById(int id)
        {
            SqlCallParameters parameters = CreateDefaultParameters(2, Actions.SelectById, id);
            DataTable dataTable = GetDataTable(parameters);

            if (dataTable.Rows.Count > 1)
                throw new Exception("should have only one row!");

            return CreateFromRow(dataTable.Rows[0]);
        }

        public override async Task<T> GetByIdAsync(int id)
        {
            SqlCallParameters parameters = CreateDefaultParameters(2, Actions.SelectById, id);
            DataTable dataTable = await GetDataTableAsync(parameters);
            if (dataTable.Rows.Count > 1)
                throw new Exception("should have only one row!");

            return CreateFromRow(dataTable.Rows[0]);
        }

        public override int Insert(T item)
        {
            SqlCallParameters parameters = item.CreateParameters(CreateDefaultParameters(item.ParametersCount, Actions.Insert));

            return ExecuteStoredProcedure(parameters);
        }

        public override int InsertMany(T[] items)
        {
            int res = 0;
            if (items.IsEmpty())
                return res;
            try
            {
                SqlCallParameters[] para1 = new SqlCallParameters[items.Length];

                for (int i = 0; i < items.Length; i++)
                {
                    para1[i] = CreateDefaultParameters(items[i].ParametersCount, Actions.InsertMany);
                }

                SqlCallParameters[] para = para1;

                DataTable dt = para.ToDataTable();

                using SqlConnection con = new(cConnectionString);
                using SqlCommand cmd = new("INSERT_MANY", con)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.Parameters.AddWithValue("DATA_TABLE", dt);
                con.Open();
                res = cmd.ExecuteNonQuery();
                con.Close();
            }
            catch (Exception e)
            {
                throw;
            }

            return res;

        }

        public override async Task<int> InsertAsync(T item)
        {
            SqlCallParameters parameters = item.CreateParameters(CreateDefaultParameters(item.ParametersCount, Actions.Insert));
            return await ExecuteStoredProcedureAsync(parameters);
        }

        public override void DeleteItem(T item)
        {
            SqlCallParameters parameters = item.CreateParameters(CreateDefaultParameters(item.ParametersCount, Actions.DeleteItem));
            ExecuteStoredProcedure(parameters);
        }

        public override async Task DeleteItemAsync(T item)
        {
            SqlCallParameters parameters = item.CreateParameters(CreateDefaultParameters(item.ParametersCount, Actions.DeleteItem));
            await ExecuteStoredProcedureAsync(parameters);
        }

        public override void DeleteById(int id)
        {
            SqlCallParameters parameters = CreateDefaultParameters(2, Actions.DeleteById, id);
            ExecuteStoredProcedure(parameters);
        }

        public override async Task DeleteByIdAsync(int id)
        {
            SqlCallParameters parameters = CreateDefaultParameters(2, Actions.DeleteById, id);
            await ExecuteStoredProcedureAsync(parameters);
        }

        public override int Update(T item)
        {
            SqlCallParameters parameters = item.CreateParameters(CreateDefaultParameters(item.ParametersCount, Actions.Update));
            return ExecuteStoredProcedure(parameters);
        }

        public override async Task<int> UpdateAsync(T item)
        {
            SqlCallParameters parameters = item.CreateParameters(CreateDefaultParameters(item.ParametersCount, Actions.Update));
            return await ExecuteStoredProcedureAsync(parameters);
        }

        public override int Upsert(T item)
        {
            SqlCallParameters parameters = item.CreateParameters(CreateDefaultParameters(item.ParametersCount, Actions.Upsert));
            return ExecuteStoredProcedure(parameters);
        }

        public override async Task<int> UpsertAsync(T item)
        {
            SqlCallParameters parameters = item.CreateParameters(CreateDefaultParameters(item.ParametersCount, Actions.Upsert));
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

        private SqlCallParameters CreateDefaultParameters(int parametersCount, Actions action, string storedProcedure)
        {
            SqlCallParameters parameters = new(parametersCount, storedProcedure, action);
            return parameters;
        }

        private SqlCallParameters CreateDefaultParameters(int parametersCount, Actions action, string storedProcedure, int id)
        {
            SqlCallParameters parameters = CreateDefaultParameters(parametersCount, action, storedProcedure);
            parameters.AddParameter(ParameterNames.cId, id, DataType.Int, Direction.InputOutput);
            return parameters;
        }

        protected abstract T CreateFromRow(DataRow dataTableRow);

        private static async Task<DataTable> GetDataTableAsync(SqlCallParameters parameters)
        {
            try
            {
                await using SqlConnection con = new(cConnectionString);
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