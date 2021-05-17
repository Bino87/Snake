using DataAccessLibrary.DataAccessors;
using DataAccessLibrary.DataTransferObjects;
using DataAccessLibrary.Internal.ParameterNames;
using DataAccessLibrary.Internal.SQL.Enums;

namespace DataAccessLibrary.Helpers.SQL.HelperModules.Base.NetworkWeightAndBias
{
    internal class WeightAndBiasSqlGetByIdCreator<T> : DefaultSqlGetByIdCreator<T> where T : SqlDataTransferObject
    {
        public WeightAndBiasSqlGetByIdCreator(SqlDatabaseAccessAbstract<T> access, T item, Table table) : base(access, item, table)
        {
        }

        protected override void CreateStoredProcedureBody()
        {
            const string id = ParameterNames.SQL.cId;
            const string internalIndex = ParameterNames.SQL.cInternalIndex;
            const string layerId = ParameterNames.SQL.cLayerID;
            const string value = ParameterNames.SQL.cValue;

            const string t1 = "A";
            const string t2 = "B";

           
            AppendLine($"SELECT {t1}.{id},{t1}.{internalIndex},{t1}.{layerId},{t2}.{value}  FROM [dbo].{_table} {t1}");
            AppendLine($"LEFT JOIN {Table.NETWORK_VALUES} {t2}");
            AppendLine($"ON {t1}.{ParameterNames.SQL.cValueId} = {t2}.{ParameterNames.SQL.cId}");
            AppendLine($"WHERE {t1}.{id} = @{id}");

        }
    }
}