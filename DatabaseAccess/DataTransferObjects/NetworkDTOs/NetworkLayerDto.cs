using System.Collections.Generic;
using System.Data;
using Commons.Extensions;
using DataAccessLibrary.Internal.SQL;
using DataAccessLibrary.Internal.SQL.Enums;
using DataAccessLibrary.Internal.SQL.ParameterNames;

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
            ActivationFunctionId = row.GetAsInt(ParameterNames.cActivationFunctionId);
            NetworkId = row.GetAsInt(ParameterNames.cNetworkId);
            NumberOfNodes = row.GetAsInt(ParameterNames.cNumberOfNodes);
        }

        internal override SqlCallParameters CreateParameters(SqlCallParameters parameters)
        {
            parameters.AddParameter(ParameterNames.cActivationFunctionId, ActivationFunctionId, DataType.Int, Direction.Input);
            parameters.AddParameter(ParameterNames.cNetworkId, NetworkId, DataType.Int, Direction.Input);
            parameters.AddParameter(ParameterNames.cNumberOfNodes, NumberOfNodes, DataType.Int, Direction.Input);

            return base.CreateParameters(parameters);
        }

        internal override void FillDataRow(DataRow row)
        {
            base.FillDataRow(row);
            row[ParameterNames.cActivationFunctionId] = ActivationFunctionId;
            row[ParameterNames.cNetworkId] = NetworkId;
            row[ParameterNames.cNumberOfNodes] = NumberOfNodes;
        }

        internal override IEnumerable<string> ColumnNames()
        {
            foreach (string columnName in base.ColumnNames())
            {
                yield return columnName;
            }

            yield return ParameterNames.cActivationFunctionId;
            yield return ParameterNames.cNetworkId;
            yield return ParameterNames.cNumberOfNodes;
        }
    }
}