using System.Collections.Generic;
using System.Data;
using Commons.Extensions;
using DataAccessLibrary.Internal.ParameterNames;
using DataAccessLibrary.Internal.SQL;
using DataAccessLibrary.Internal.SQL.Enums;


namespace DataAccessLibrary.DataTransferObjects.NetworkDTOs
{
    public class InternalyIndexedDto : SqlDataTransferObject
    {
        public int InternalIndex { get; }
        internal override int ParametersCount => base.ParametersCount + 1;

        internal InternalyIndexedDto() : base()
        {
            
        }

        protected  InternalyIndexedDto(DataRow row) : base(row)
        {
            InternalIndex = row.GetAsInt(ParameterNames.SQL.cInternalIndex);
        }

        internal override SqlCallParameters CreateParameters(SqlCallParameters parameters)
        {
            parameters.AddParameter(ParameterNames.SQL.cInternalIndex, InternalIndex, DataType.Int, Direction.Input);

            return base.CreateParameters(parameters);
        }

        internal override void FillDataRow(DataRow row)
        {
            base.FillDataRow(row);
            row[ParameterNames.SQL.cInternalIndex] = InternalIndex;
        }

        internal override IEnumerable<string> ColumnNames()
        {
            foreach(string columnName in base.ColumnNames())
            {
                yield return columnName;
            }

            yield return ParameterNames.SQL.cInternalIndex;
        }
    }
}