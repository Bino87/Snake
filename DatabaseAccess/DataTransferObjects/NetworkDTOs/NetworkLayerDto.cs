using System.Collections.Generic;
using System.Data;
using Commons.Extensions;
using DataAccessLibrary.Internal.ParameterNames;
using DataAccessLibrary.Internal.SQL;
using DataAccessLibrary.Internal.SQL.Enums;


namespace DataAccessLibrary.DataTransferObjects.NetworkDTOs
{
    public class NetworkLayerDto : InternallyIndexedDto
    {
        public int ActivationFunctionId { get; }
        public int NetworkId { get; }
        public int NumberOfNodes { get; }

        internal override int ParametersCount => base.ParametersCount + 3;

        internal NetworkLayerDto() : base()
        {

        }

        public NetworkLayerDto(int networkId, int activationFunctionId, int numberOfNodes, int index) : base(index)
        {
            NetworkId = networkId;
            ActivationFunctionId = activationFunctionId;
            NumberOfNodes = numberOfNodes;
        }

        internal NetworkLayerDto(DataRow row) : base(row)
        {
            ActivationFunctionId = row.GetAsInt(ParameterNames.SQL.cActivationFunctionId);
            NetworkId = row.GetAsInt(ParameterNames.SQL.cNetworkId);
            NumberOfNodes = row.GetAsInt(ParameterNames.SQL.cNumberOfNodes);
        }

        internal override void FillDataRow(DataRow row)
        {
            base.FillDataRow(row);
            row[ParameterNames.SQL.cActivationFunctionId] = ActivationFunctionId;
            row[ParameterNames.SQL.cNetworkId] = NetworkId;
            row[ParameterNames.SQL.cNumberOfNodes] = NumberOfNodes;
        }

        internal override IEnumerable<ColumnDefinition> ColumnNames()
        {
            foreach (ColumnDefinition cd in base.ColumnNames())
            {
                yield return cd;
            }

            yield return new(ParameterNames.SQL.cActivationFunctionId, ActivationFunctionId, DataType.Int, Direction.Input);
            yield return new(ParameterNames.SQL.cNetworkId, NetworkId, DataType.Int, Direction.Input);
            yield return new(ParameterNames.SQL.cNumberOfNodes, NumberOfNodes, DataType.Int, Direction.Input);
        }
    }
}