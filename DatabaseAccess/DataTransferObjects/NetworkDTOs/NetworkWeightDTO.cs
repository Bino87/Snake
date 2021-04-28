using System.Collections.Generic;
using System.Data;
using Commons.Extensions;
using DataAccessLibrary.Internal.SQL;
using DataAccessLibrary.Internal.SQL.Enums;
using DataAccessLibrary.Internal.SQL.ParameterNames;

namespace DataAccessLibrary.DataTransferObjects.NetworkDTOs
{
    public class NetworkWeightDto : SqlDataTransferObject
    {
        public int LayerId { get; set; }
        public double Value { get; set; }
        public int InternalIndex { get; set; }

        internal override int ParametersCount => base.ParametersCount + 3;

        internal NetworkWeightDto(DataRow row) : base(row)
        {
            Value = row.GetAsDouble(ParameterNames.cValue);
            InternalIndex = row.GetAsInt(ParameterNames.cInternalIndex);
            LayerId = row.GetAsInt(ParameterNames.cLayerID);
        }


        internal override SqlCallParameters CreateParameters(SqlCallParameters parameters)
        {
            parameters.AddParameter(ParameterNames.cValue, Value, DataType.Double, Direction.Input);
            parameters.AddParameter(ParameterNames.cLayerID, LayerId, DataType.Int, Direction.Input);
            parameters.AddParameter(ParameterNames.cInternalIndex, InternalIndex, DataType.Int, Direction.Input);

            return base.CreateParameters(parameters);
        }

        internal override void FillDataRow(DataRow row)
        {
            base.FillDataRow(row);
            row[ParameterNames.cValue] = Value;
            row[ParameterNames.cLayerID] = LayerId;
            row[ParameterNames.cInternalIndex] = InternalIndex;

        }

        internal override IEnumerable<string> ColumnNames()
        {
            foreach(string columnName in base.ColumnNames())
            {
                yield return columnName;
            }

            yield return ParameterNames.cInternalIndex;
            yield return ParameterNames.cLayerID;
            yield return ParameterNames.cValue;
        }
    }
}
