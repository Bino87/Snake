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

        public static double InverseLerp(this int value, double min, double max) => ((double)value).InverseLerp(min, max);

        public static float ToFloat(this double d) => (float)d;
    }
}