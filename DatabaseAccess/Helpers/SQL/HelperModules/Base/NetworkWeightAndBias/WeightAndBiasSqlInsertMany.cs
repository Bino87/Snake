using DataAccessLibrary.DataAccessors;
using DataAccessLibrary.DataTransferObjects;
using DataAccessLibrary.Internal.ParameterNames;
using DataAccessLibrary.Internal.SQL.Enums;

namespace DataAccessLibrary.Helpers.SQL.HelperModules.Base.NetworkWeightAndBias
{
    internal class WeightAndBiasSqlInsertMany<T> : DefaultSqlInsertManyCreator<T> where T : SqlDataTransferObject
    {
        private const string dataTable = ParameterNames.SQL.cDataTable;
        private const string dt = "dt";
        private const string values = ParameterNames.SQL.cValues;
        private const string value = ParameterNames.SQL.cValue;
        private const string nv = "nv";
        private const string v = "v";
        private const string id = ParameterNames.SQL.cId;
        private const string internalIndex = ParameterNames.SQL.cInternalIndex;
        private const string valueId = ParameterNames.SQL.cValueId;
        private const string layerId = ParameterNames.SQL.cLayerID;

        public WeightAndBiasSqlInsertMany(SqlDatabaseAccessAbstract<T> access, T item, Table table) : base(access, item, table)
        {
        }

        protected override void CreateStoredProcedureBody()
        {
           
            
            AppendLine($"\tDECLARE @{values} [dbo].[{Table.NETWORK_VALUES}_TYPE]");

            InsertUnknownToNetworkValues();
            InsertKnownToTempTable();
            InsertIntoTable();
            ReturnValue();
           
        }

        private void ReturnValue()
        {
            AppendLine("-- return values");
            AppendLine($"\tSELECT {dt}.{id}, {dt}.{internalIndex}, {dt}.{layerId}, {v}.{valueId} FROM @{dataTable} {dt}");
            AppendLine($"\tLEFT JOIN @{values} {v}");
            AppendLine($"\tON {v}.{internalIndex} = {dt}.{internalIndex} AND {v}.{layerId} = {dt}.{layerId} AND {v}.{id} IS NOT NULL");
        }

        private void InsertIntoTable()
        {
            AppendLine("-- insert into table");
            AppendLine($"\tINSERT INTO {_table} ({string.Join(", ", internalIndex, layerId, valueId)})");
            AppendLine($"\tSELECT {v}.{internalIndex}, {v}.{layerId}, {v}.{valueId} FROM @{values} {v}");
        }

        private void InsertKnownToTempTable()
        {
            AppendLine();
            AppendLine($"--insert known to temp table @{values}");
            AppendLine($"\tINSERT INTO @{values} ({string.Join(", ", id, internalIndex, valueId, layerId)})");
            AppendLine($"\tSELECT 0, {dt}.{internalIndex}, {nv}.{id}, {dt}.{layerId} from @{dataTable} {dt}");
            AppendLine($"\tINNER JOIN {Table.NETWORK_VALUES} {nv}");
            AppendLine($"\tON {dt}.{value} = {nv}.{value}");
        }

        private void InsertUnknownToNetworkValues()
        {
            AppendLine();
            AppendLine("--insert unknown into network values");
            AppendLine($"\tINSERT INTO {Table.NETWORK_VALUES}");
            AppendLine($"\tSELECT DISTINCT {dt}.{value} from @{dataTable} {dt}");
            AppendLine($"\tLEFT JOIN {Table.NETWORK_VALUES} {nv}");
            AppendLine($"\tON {nv}.{value} = {dt}.{value} WHERE {nv}.{value} IS NULL");
        }
    }
}