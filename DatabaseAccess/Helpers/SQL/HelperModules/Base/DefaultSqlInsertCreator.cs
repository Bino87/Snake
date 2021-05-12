using DataAccessLibrary.DataAccessors;
using DataAccessLibrary.DataTransferObjects;
using DataAccessLibrary.Internal.ParameterNames;
using DataAccessLibrary.Internal.SQL.Enums;

namespace DataAccessLibrary.Helpers.SQL.HelperModules.Base
{
    internal class WeightAndBiasSqlInsertCreator<T> : DefaultSqlInsertCreator<T> where T : SqlDataTransferObject
    {
        public WeightAndBiasSqlInsertCreator(SqlDatabaseAccessAbstract<T> access, T item, Table table) : base(access, item, table)
        {
        }

        protected override void CreateBody()
        {
            const string id = ParameterNames.SQL.cId;
            const string value = ParameterNames.SQL.cValue;
            const string valueId = ParameterNames.SQL.cValueId;
            const string internalIndex = ParameterNames.SQL.cInternalIndex;
            const string layerId = ParameterNames.SQL.cLayerID;


            _sb.AppendLine($"IF NOT EXISTS(SELECT TOP(1) {id} ");
            _sb.AppendLine($"FROM) {Table.NETWORK_VALUE} V");
            _sb.AppendLine($"WHERE V.{value} = {value})");
            _sb.AppendLine("BEGIN");

            _sb.AppendLine("DECLARE @NEW_ID INT;");
            _sb.AppendLine($"INSERT INTO {Table.NETWORK_VALUE} ({value})");
            _sb.AppendLine($"VALUES(@{value})");
            _sb.AppendLine("SET @NEW_ID = SCOPE_IDENTITY()");
            _sb.AppendLine();
            _sb.AppendLine($"INSERT INTO {_table} VALUES(@{internalIndex},@NEW_ID,@{layerId})");

            _sb.AppendLine("END");
            _sb.AppendLine("ELSE");
            _sb.AppendLine("BEGIN");

            _sb.AppendLine($"INSERT INTO {_table} (@{internalIndex},@{valueId},@{layerId})");
            _sb.AppendLine($"SELECT TOP(1) @{internalIndex},@{layerId},{id}");
            _sb.AppendLine($"FROM {Table.NETWORK_VALUE} V");
            _sb.AppendLine($"WHERE V.{value} = @{value}");
            _sb.AppendLine("END");

            _sb.AppendLine("SET @ID = SCOPE_IDENTITY()");
            _sb.AppendLine("RETURN @ID");

        }

    }

    internal class DefaultSqlInsertCreator<T> : SqlStoredProcedureCreator<T> where T : SqlDataTransferObject
    {
        public DefaultSqlInsertCreator(SqlDatabaseAccessAbstract<T> access, T item, Table table) : base(access, item, table, Actions.INSERT)
        {
        }

        protected override void CreateBody()
        {
            _sb.AppendLine("AS");
            _sb.AppendLine("BEGIN");
            _sb.AppendLine("\tSET NOCOUNT ON");
            _sb.AppendLine("");
            _sb.AppendLine($"\tINSERT INTO {_table} VALUES({GetParameterNames(false, "@")})");
            _sb.AppendLine("\tSET @ID = SCOPE_IDENTITY();");
            _sb.AppendLine("\tRETURN @ID");

            _sb.AppendLine("END");
        }

        protected override void CreateParameters()
        {
            _sb.AppendLine(GetParametrized());
        }
    }
}