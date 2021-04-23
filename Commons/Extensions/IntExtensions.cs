using System;

namespace Commons.Extensions
{
    public static class IntExtensions
    {
        public static int Pow2(this int i) => i * i;
    }

    public static class StringExtensions
    {
        public static byte ToByte(this string str, int fromBase) => Convert.ToByte(str, fromBase);
    }
}