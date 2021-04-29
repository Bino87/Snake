using System.Collections.Generic;
using System.Data;
using Commons.Extensions;
using DataAccessLibrary.Internal.SQL;
using DataAccessLibrary.Internal.SQL.Enums;
using DataAccessLibrary.Internal.SQL.ParameterNames;

namespace DataAccessLibrary.DataTransferObjects.NetworkDTOs
{
    public class NetworkWeightDto : InternalyIndexedDto
    {
        public int LayerId { get; set; }
        public double Value { get; set; }

        internal override int ParametersCount => base.ParametersCount + 2;

        internal NetworkWeightDto() : base()
        {

        }
        internal NetworkWeightDto(DataRow row) : base(row)
        {
            Value = row.GetAsDouble(ParameterNames.cValue);
            LayerId = row.GetAsInt(ParameterNames.cLayerID);
        }


        internal override SqlCallParameters CreateParameters(SqlCallParameters parameters)
        {
            parameters.AddParameter(ParameterNames.cValue, Value, DataType.Double, Direction.Input);
            parameters.AddParameter(ParameterNames.cLayerID, LayerId, DataType.Int, Direction.Input);

            return base.CreateParameters(parameters);
        }

        internal override void FillDataRow(DataRow row)
        {
            base.FillDataRow(row);
            row[ParameterNames.cValue] = Value;
            row[ParameterNames.cLayerID] = LayerId;

        }

        internal override IEnumerable<string> ColumnNames()
        {
            foreach(string columnName in base.ColumnNames())
            {
                yield return columnName;
            }

            yield return ParameterNames.cLayerID;
            yield return ParameterNames.cValue;
        }
    }
}
