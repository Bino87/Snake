using System;
using System.Data;

namespace DataAccessLibrary.Internal.SQL.Enums
{
    public enum Table
    {
        TESTTABLE = 1,
    }

    internal enum Actions
    {
        GET_ALL = 1,
        GET_BY_ID = 2,
        INSERT = 3,
        DELETE_BY_ID = 4,
        DELETE_ITEM = 5,
        UPDATE = 6,
        UPSERT = 7,
        INSERT_MANY = 8,
        UPDATE_MANY = 9,
        UPSERT_MANY = 10,
    }

    internal enum DataType
    {
        Int,
        String,
        Double,
        TimeStamp,
        DateTime,
    }

    internal enum Direction
    {
        Input,
        Output,
        InputOutput,
        ReturnValue
    }

    internal static class EnumExtensions
    {
        internal static int ToActionId(this Actions action)
        {
            return (int) action;
        }

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
                _ => throw new ArgumentOutOfRangeException(nameof(dt), dt, null)
            };
        }
    }

}
