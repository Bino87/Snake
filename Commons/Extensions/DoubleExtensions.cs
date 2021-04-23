using System;

namespace Commons.Extensions
{
    public static class DoubleExtensions
    {
        public static double Lerp(this double value, double min, double max) => min * (1 - value) + max * value;

        public static double InverseLerp(this double value, double min, double max)
        {
            if (value <= min)
                return 0;
            if (value >= max)
                return 1;
            return (value - min) / (max - min);
        }

        public static double Pow2(this double value) => value * value;

        public static double InverseLerp(this int value, double min, double max) => ((double)value).InverseLerp(min, max);

        public static float ToFloat(this double d) => (float)d;

        public static byte[] GetBytes(this double d) => BitConverter.GetBytes(d);

        public static double Clamp(this double v, double min, double max)
        {
            if (v <= min) return min;
            if (v >= max) return max;
            return v;
        }

        public static double Clamp1Neg1(this double d) => d.Clamp(-1, 1);

        public static double Clamp01(this double v) => v.Clamp(0, 1);

        public static bool IsOkNumber(this double d)
        {
            if (double.IsNaN(d)) return false;
            if (double.IsInfinity(d)) return false;
            return true;
        }
    }
}