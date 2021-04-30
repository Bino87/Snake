using System;

namespace Commons.Extensions
{
    public static class IntExtensions
    {
        public static int Pow2(this int i) => i * i;
        public static int Abs(this int value) => Math.Abs(value);

        public static int Clamp(this int value, int min, int max)
        {
            if (min > value) return min;
            if (max < value) return max;
            return value;
        }
    }
}