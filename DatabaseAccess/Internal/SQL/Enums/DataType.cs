using System;
using System.Data;

namespace DataAccessLibrary.Internal.SQL.Enums
{
    internal enum SQL_STORED_PROCEDURE
    {

    }

    internal enum Actions
    {
        SelectAll = 1,
        SelectById = 2,
        Insert = 3,
        DeleteById = 4,
        DeleteItem = 5,
        Update = 6,
        Upsert = 7,
        InsertMany = 8,
        UpdateMany = 9,
        UpsertMany = 10,
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
