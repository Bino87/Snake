using System.Data.SqlClient;
using DataAccessLibrary.Extensions;
using DataAccessLibrary.Internal.SQL.Enums;

namespace DataAccessLibrary.Internal.SQL
{
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