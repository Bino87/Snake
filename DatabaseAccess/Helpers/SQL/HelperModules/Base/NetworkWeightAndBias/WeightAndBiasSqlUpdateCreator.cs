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

        protected override void CreateStoredProcedureBody()
        {
            const string id = ParameterNames.SQL.cId;
            const string value = ParameterNames.SQL.cValue;
            const string valueId = ParameterNames.SQL.cValueId;
            const string newId = ParameterNames.SQL.cNewId;



            AppendLine($"DECLARE @{newId} INT;");

            AppendLine($"IF NOT EXISTS(SELECT TOP(1) V.{id} ");
            AppendLine($"FROM {Table.NETWORK_VALUES} V");
            AppendLine($"WHERE V.{value} = {value})");
            AppendLine("BEGIN");

            AppendLine($"INSERT INTO {Table.NETWORK_VALUES} ({value})");
            AppendLine($"VALUES(@{value})");
            AppendLine("SET @NEW_ID = SCOPE_IDENTITY()");
            AppendLine("END");
            AppendLine("ELSE");
            AppendLine("BEGIN");
            AppendLine($"SET @{newId} = (SELECT TOP(1) {id} FROM {Table.NETWORK_VALUES} V WHERE V.{value} = @{value})");
            AppendLine("END");

            AppendLine($"UPDATE {_table} SET");
            AppendLine($"{valueId} = @{newId}");
            AppendLine($"WHERE {id} = @{id}");
            AppendLine($"SET @{id} = SCOPE_IDENTITY();");
            AppendLine($"RETURN @{id}");
        }
    }
}