using System;
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
            Id = row.GetAsInt(ParameterNames.SQL.cId);
        }

        internal SqlCallParameters CreateParameters(SqlCallParameters parameters)
        {
            foreach (ColumnDefinition columnDefinition in ColumnNames())
            {
                parameters.AddParameter(columnDefinition.ToSqlCallParameter());
            }

            return parameters;
        }

        internal virtual void FillDataRow(DataRow row)
        {
            row[ParameterNames.SQL.cId] = Id;
        }

        internal virtual IEnumerable<ColumnDefinition> ColumnNames()
        {
            yield return new(ParameterNames.SQL.cId, Id, DataType.Int, Direction.InputOutput);
        }
    }
}