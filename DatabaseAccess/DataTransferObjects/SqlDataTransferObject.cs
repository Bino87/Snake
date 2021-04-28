using System;
using System.Collections.Generic;
using System.Data;
using Commons.Extensions;
using DataAccessLibrary.Internal.Enums;
using DataAccessLibrary.Internal.SQL;
using DataAccessLibrary.Internal.SQL.Enums;
using DataAccessLibrary.Internal.SQL.ParameterNames;

namespace DataAccessLibrary.DataTransferObjects
{
    public class SqlDataTransferObject : DataTransferObject
    {
        internal int Id { get; }
        internal override DatabaseType DbType => DatabaseType.Sql;
        internal virtual int ParametersCount { get; }

        internal SqlDataTransferObject(DataRow row)
        {
            Id = row.GetAsInt(ParameterNames.cId);
        }

        internal SqlCallParameters CreateParameters(SqlCallParameters parameters)
        {
            parameters.AddParameter(ParameterNames.cId, Id, DataType.Int, Direction.InputOutput);
            return parameters;
        }

        internal virtual void FillDataRow(DataRow dataRow)
        {
            dataRow[ParameterNames.cId] = Id;
        }

        internal virtual IEnumerable<string> ColumnNames()
        {
            yield return ParameterNames.cId;
        }

    }

    public class MongoDbDataTransferObject : DataTransferObject
    {
        internal Guid Id { get; }
        internal override DatabaseType DbType => DatabaseType.MongoDB;
    }
}