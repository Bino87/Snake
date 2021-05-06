using System;
using System.Data.SqlClient;
using DataAccessLibrary.Extensions;
using DataAccessLibrary.Internal.SQL.Enums;

namespace DataAccessLibrary.Internal.SQL
{
    internal record ColumnDefinition(string Name, object Value, DataType DataType, Direction Direction)
    {
        internal SqlCallParameter ToSqlCallParameter() => new(Name, Value, DataType, Direction);
    }

    internal record SqlCallParameter(string ParameterName, object Value, DataType DataType, Direction Direction)
    {
        internal SqlParameter ToSqlParameter() => new()
            {
                Direction = Direction.ToParameterDirection(),
                ParameterName = ParameterName,
                Value = Value,
                SqlDbType = DataType.ToSqlType()
            };
    }
}