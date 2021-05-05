using System;

namespace Commons.Extensions
{
    public static class EnumExtensions
    {
        public static T CastTo<T>(this Enum value) where T : Enum => value is T @enum ? @enum : throw new Exception("Unable to cast to enum");

    }
    public static class StringExtensions
    {
        public static byte ToByte(this string str, int fromBase) => Convert.ToByte(str, fromBase);

        public static bool IsNullOrWhiteSpace(this string str) => string.IsNullOrWhiteSpace(str);

        public static bool IsNotNullOrWhiteSpace(this string str) => !str.IsNullOrWhiteSpace();

        public static bool TryParse(this string str, out int value) => int.TryParse(str, out value);

        public static bool TryParse(this string str, out float value) => float.TryParse(str, out value);

        public static bool TryParse(this string str, out double value) => double.TryParse(str, out value);

        public static bool TryParse(this string str, out bool b) => bool.TryParse(str, out b);

        public static T Parse<T>(this string str) where T : struct, Enum => Enum.TryParse(str, out T @enum) ? @enum : throw new Exception("Unable to parse enum");
    }
}