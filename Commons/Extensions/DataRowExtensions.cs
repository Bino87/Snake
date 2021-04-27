using System;
using System.Data;
using System.Linq.Expressions;

namespace Commons.Extensions
{
    public static class DataRowExtensions
    {
        public static string GetAsString(this DataRow row, string column)
        {
            object val = row[column];

            return val?.ToString();
        }

        public static int GetAsInt(this DataRow row, string column)
        {
            string val = row.GetAsString(column);

            if (val.IsNotNullOrWhiteSpace())
            {
                if (val.TryParse(out int i))
                    return i;
            }

            throw new Exception("Parsing Failed");
        }

        public static double GetAsDouble(this DataRow row, string column)
        {
            string val = row.GetAsString(column);

            if (val.IsNotNullOrWhiteSpace())
            {
                if (val.TryParse(out double i))
                    return i;
            }

            throw new Exception("Parsing Failed");
        }

        public static T GetAsEnum<T>(this DataRow row, string column) where T : Enum
        {
            ParameterExpression parameter = Expression.Parameter(typeof(int));
            Expression<Func<int, T>> dynamicMethod = Expression.Lambda<Func<int, T>>(Expression.Convert(parameter, typeof(T)), parameter);

            return dynamicMethod.Compile()(row.GetAsInt(column));
        }
    }
}