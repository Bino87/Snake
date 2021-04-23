using System;

namespace Commons.Extensions
{
    public static class FloatExtensions
    {
        public static double Lerp(this float value, float min, float max) => min * (1 - value) + max * value;

        public static float InverseLerp(this float value, float min, float max)
        {
            if (value <= min)
                return 0;
            if (value >= max)
                return 1;
            return (value - min) / (max - min);
        }

        public static float InverseLerp(this int value, float min, float max) => ((float)value).InverseLerp(min, max);

        public static double ToDouble(this float f) => f;

        public static float Pow2(this float value) => value * value;

        public static float Clamp(this float v, float min, float max)
        {
            if (v <= min) return min;
            if (v >= max) return max;
            return v;
        }

        public static float Abs(this float value) => Math.Abs(value);

        public static float Clamp01(this float v) => v.Clamp(0, 1);

        public static byte[] GetBytes(this float d) => BitConverter.GetBytes(d);

    }
}