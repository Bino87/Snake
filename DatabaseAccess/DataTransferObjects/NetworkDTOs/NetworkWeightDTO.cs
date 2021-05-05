using System.Collections.Generic;
using System.Data;
using Commons.Extensions;
using DataAccessLibrary.Internal.ParameterNames;
using DataAccessLibrary.Internal.SQL;
using DataAccessLibrary.Internal.SQL.Enums;


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
            Value = row.GetAsDouble(ParameterNames.SQL.cValue);
            LayerId = row.GetAsInt(ParameterNames.SQL.cLayerID);
        }


        internal override SqlCallParameters CreateParameters(SqlCallParameters parameters)
        {
            parameters.AddParameter(ParameterNames.SQL.cValue, Value, DataType.Double, Direction.Input);
            parameters.AddParameter(ParameterNames.SQL.cLayerID, LayerId, DataType.Int, Direction.Input);

            return base.CreateParameters(parameters);
        }

        internal override void FillDataRow(DataRow row)
        {
            base.FillDataRow(row);
            row[ParameterNames.SQL.cValue] = Value;
            row[ParameterNames.SQL.cLayerID] = LayerId;

        }

        internal override IEnumerable<string> ColumnNames()
        {
            foreach(string columnName in base.ColumnNames())
            {
                yield return columnName;
            }

            yield return ParameterNames.SQL.cLayerID;
            yield return ParameterNames.SQL.cValue;
        }
    }
}
