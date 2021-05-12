using DataAccessLibrary.DataAccessors;
using DataAccessLibrary.DataTransferObjects;
using DataAccessLibrary.Internal.ParameterNames;
using DataAccessLibrary.Internal.SQL.Enums;

namespace DataAccessLibrary.Helpers.SQL.HelperModules.Base.NetworkWeightAndBias
{
    internal class WeightAndBiasSqlUpdateCreator<T> : DefaultSqlUpdateCreator<T> where T : SqlDataTransferObject
    {
        public WeightAndBiasSqlUpdateCreator(SqlDatabaseAccessAbstract<T> access, T item, Table table) : base(access, item, table)
        {
        }

        protected override void CreateBody()
        {
            const string id = ParameterNames.SQL.cId;
            const string value = ParameterNames.SQL.cValue;
            const string valueId = ParameterNames.SQL.cValueId;
            const string newId = ParameterNames.SQL.cNewId;

            _sb.AppendLine($"DECLARE @{newId} INT;");

            _sb.AppendLine($"IF NOT EXISTS(SELECT TOP(1) {id} ");
            _sb.AppendLine($"FROM) {Table.NETWORK_VALUE} V");
            _sb.AppendLine($"WHERE V.{value} = {value})");
            _sb.AppendLine("BEGIN");

            _sb.AppendLine($"INSERT INTO {Table.NETWORK_VALUE} ({value})");
            _sb.AppendLine($"VALUES(@{value})");
            _sb.AppendLine("SET @NEW_ID = SCOPE_IDENTITY()");
            _sb.AppendLine("END");
            _sb.AppendLine("ELSE");
            _sb.AppendLine("BEGIN");
            _sb.AppendLine($"SET @{newId} = (SELECT TOP(1) {id} FROM {Table.NETWORK_VALUE} V WHERE V.{value} = @{value})");
            _sb.AppendLine("END");

            _sb.AppendLine($"UPDATE {_table} SET");
            _sb.AppendLine($"{valueId} = @{newId}");
            _sb.AppendLine($"WHERE {id} = @{id}");
            _sb.AppendLine($"SET @{id} = SCOPE_IDENTITY();");
            _sb.AppendLine($"RETURN @{id}");
        }
    }
}