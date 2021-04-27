using System;
using System.Data;
using Commons.Extensions;
using DataAccessLibrary.Internal.Enums;
using DataAccessLibrary.Internal.SQL;
using DataAccessLibrary.Internal.SQL.Enums;
using DataAccessLibrary.Internal.SQL.ParameterNames;

namespace DataAccessLibrary.DataTransferObjects
{
    public abstract class SqlDataTransferObject : DataTransferObject
    {
        internal int Id { get; }
        internal override DatabaseType DbType => DatabaseType.Sql;
        internal abstract int ParametersCount { get; }

        internal SqlDataTransferObject(DataRow row)
        {
            Id = row.GetAsInt(ParameterNames.cId);
        }

        internal SqlCallParameters CreateParameters(SqlCallParameters parameters)
        {
            parameters.AddParameter(ParameterNames.cId, Id, DataType.Int, Direction.InputOutput);
            return parameters;
        }
    }

    public abstract class MongoDbDataTransferObject : DataTransferObject
    {
        internal Guid Id { get; }
        internal override DatabaseType DbType => DatabaseType.MongoDB;
    }
}