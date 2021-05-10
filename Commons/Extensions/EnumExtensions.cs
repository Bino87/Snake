using System;

namespace Commons.Extensions
{
    public static class EnumExtensions
    {
        public static T CastTo<T>(this Enum value) where T : Enum => value is T @enum ? @enum : throw new Exception("Unable to cast to enum");

        public static T ParseTo<T>(this string str) where T : Enum
        {
            return (T)Enum.Parse(typeof(T), str);
        }

        public static string[] ToStringArray<T>(this T[] @enum) where T : Enum
        {
            string[] arr = new string[@enum.Length];

            for (int i = 0; i < @enum.Length; i++)
            {
                arr[i] = @enum[i].ToString();
            }

            return arr;
        }

        public static T[] FromStringArray<T>(this string[] arr) where T : Enum
        {
            return Array.ConvertAll(arr, x => (T)Enum.Parse(typeof(T), x));
        }

    }
}