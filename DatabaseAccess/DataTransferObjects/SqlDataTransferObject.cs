using System.Collections.Generic;
using System.Data;
using Commons.Extensions;
using DataAccessLibrary.Internal.Enums;
using DataAccessLibrary.Internal.ParameterNames;
using DataAccessLibrary.Internal.SQL;
using DataAccessLibrary.Internal.SQL.Enums;


namespace DataAccessLibrary.DataTransferObjects
{
    public class SqlDataTransferObject : DataTransferObject<int>
    {
        internal override DatabaseType DbType => DatabaseType.Sql;
        internal virtual int ParametersCount => 1;

        protected SqlDataTransferObject() { }
        protected SqlDataTransferObject(DataRow row)
        {
            Id = row.GetAsInt(ParameterNames.cSqlId);
        }

        internal virtual SqlCallParameters CreateParameters(SqlCallParameters parameters)
        {
            parameters.AddParameter(ParameterNames.cSqlId, Id, DataType.Int, Direction.InputOutput);
            return parameters;
        }

        internal virtual void FillDataRow(DataRow row)
        {
            row[ParameterNames.cSqlId] = Id;
        }

        internal virtual IEnumerable<string> ColumnNames()
        {
            yield return ParameterNames.cSqlId;
        }
    }
}