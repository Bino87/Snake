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

    }
}