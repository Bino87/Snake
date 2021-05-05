using System;
using System.Data;
using DataAccessLibrary.Internal.SQL.Enums;

namespace DataAccessLibrary.Extensions
{
    internal static class EnumExtensions
    {
        internal static string CreateStoredProcedureName(this Table table, Actions action) =>
            string.Join("_", table, action);


        internal static ParameterDirection ToParameterDirection(this Direction direction)
        {
            return direction switch
            {
                Direction.Input => ParameterDirection.Input,
                Direction.Output => ParameterDirection.Output,
                Direction.InputOutput => ParameterDirection.InputOutput,
                Direction.ReturnValue => ParameterDirection.ReturnValue,
                _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
            };
        }

        internal static SqlDbType ToSqlType(this DataType dt)
        {
            return dt switch
            {
                DataType.Int => SqlDbType.Int,
                DataType.String => SqlDbType.VarChar,
                DataType.Double => SqlDbType.Float,
                DataType.TimeStamp => SqlDbType.BigInt,
                DataType.DateTime => SqlDbType.DateTime2,
                DataType.String25 => SqlDbType.VarChar,
                DataType.String50 => SqlDbType.VarChar,
                _ => throw new ArgumentOutOfRangeException(nameof(dt), dt, null)
            };
        }
    }
}