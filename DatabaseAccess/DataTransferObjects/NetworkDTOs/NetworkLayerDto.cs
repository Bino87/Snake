using System.Collections.Generic;
using System.Data;
using Commons.Extensions;
using DataAccessLibrary.Internal.ParameterNames;
using DataAccessLibrary.Internal.SQL;
using DataAccessLibrary.Internal.SQL.Enums;


namespace DataAccessLibrary.DataTransferObjects.NetworkDTOs
{
    public class NetworkLayerDto : InternalyIndexedDto
    {
        public int ActivationFunctionId { get; }
        public int NetworkId { get; }
        public int NumberOfNodes { get; }

        internal override int ParametersCount => base.ParametersCount + 3;

        internal NetworkLayerDto() : base()
        {
            
        }

        internal NetworkLayerDto(DataRow row) : base(row)
        {
            ActivationFunctionId = row.GetAsInt(ParameterNames.SQL.cActivationFunctionId);
            NetworkId = row.GetAsInt(ParameterNames.SQL.cNetworkId);
            NumberOfNodes = row.GetAsInt(ParameterNames.SQL.cNumberOfNodes);
        }

        internal override SqlCallParameters CreateParameters(SqlCallParameters parameters)
        {
            parameters.AddParameter(ParameterNames.SQL.cActivationFunctionId, ActivationFunctionId, DataType.Int, Direction.Input);
            parameters.AddParameter(ParameterNames.SQL.cNetworkId, NetworkId, DataType.Int, Direction.Input);
            parameters.AddParameter(ParameterNames.SQL.cNumberOfNodes, NumberOfNodes, DataType.Int, Direction.Input);

            return base.CreateParameters(parameters);
        }

        internal override void FillDataRow(DataRow row)
        {
            base.FillDataRow(row);
            row[ParameterNames.SQL.cActivationFunctionId] = ActivationFunctionId;
            row[ParameterNames.SQL.cNetworkId] = NetworkId;
            row[ParameterNames.SQL.cNumberOfNodes] = NumberOfNodes;
        }

        internal override IEnumerable<string> ColumnNames()
        {
            foreach (string columnName in base.ColumnNames())
            {
                yield return columnName;
            }

            yield return ParameterNames.SQL.cActivationFunctionId;
            yield return ParameterNames.SQL.cNetworkId;
            yield return ParameterNames.SQL.cNumberOfNodes;
        }
    }
}