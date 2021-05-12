using DataAccessLibrary.DataAccessors;
using DataAccessLibrary.DataTransferObjects;
using DataAccessLibrary.Internal.ParameterNames;
using DataAccessLibrary.Internal.SQL.Enums;

namespace DataAccessLibrary.Helpers.SQL.HelperModules.Base.NetworkWeightAndBias
{
    internal class WeightAndBiasSqlInsertCreator<T> : DefaultSqlInsertCreator<T> where T : SqlDataTransferObject
    {
        public WeightAndBiasSqlInsertCreator(SqlDatabaseAccessAbstract<T> access, T item, Table table) : base(access, item, table)
        {
        }

        protected override void CreateBody()
        {
            const string value = ParameterNames.SQL.cValue;
            const string valueId = ParameterNames.SQL.cValueId;
            const string internalIndex = ParameterNames.SQL.cInternalIndex;
            const string layerId = ParameterNames.SQL.cLayerID;


            _sb.AppendLine($"DECLARE @{valueId} INT, @RETURN_VALUE INT");
            _sb.AppendLine($"EXEC @{valueId} = [dbo].[dbo].[NETWORK_VALUE_TRY_INSERT] @{value} = @{value}");
            _sb.AppendLine($"INSERT INTO {_table} ({internalIndex},{layerId},{valueId})");
            _sb.AppendLine($"VALUES(@{internalIndex},@{layerId},@{valueId})");

            _sb.AppendLine("RETURN @RETURN_VALUE");

        }

    }
}