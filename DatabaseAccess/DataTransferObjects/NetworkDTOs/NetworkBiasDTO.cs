using System.Collections.Generic;
using System.Data;
using Commons.Extensions;
using DataAccessLibrary.Internal.ParameterNames;
using DataAccessLibrary.Internal.SQL;
using DataAccessLibrary.Internal.SQL.Enums;


namespace DataAccessLibrary.DataTransferObjects.NetworkDTOs
{
    public class NetworkBiasDto : InternalyIndexedDto
    {
        public int LayerId { get; set; }
        public double Value { get; set; }

        internal override int ParametersCount => base.ParametersCount + 2;

        public NetworkBiasDto() : base()
        {

        }

        internal NetworkBiasDto(DataRow row) : base(row)
        {
            Value = row.GetAsDouble(ParameterNames.SQL.cValue);
            LayerId = row.GetAsInt(ParameterNames.SQL.cLayerID);
        }

        internal override void FillDataRow(DataRow row)
        {
            base.FillDataRow(row);
            row[ParameterNames.SQL.cValue] = Value;
            row[ParameterNames.SQL.cLayerID] = LayerId;
        }

        internal override IEnumerable<ColumnDefinition> ColumnNames()
        {
            foreach (ColumnDefinition cd in base.ColumnNames())
            {
                yield return cd;
            }

            yield return new ColumnDefinition(ParameterNames.SQL.cValue, Value, DataType.Double, Direction.Input);
            yield return new ColumnDefinition(ParameterNames.SQL.cLayerID, LayerId, DataType.Int, Direction.Input);
        }
    }
}