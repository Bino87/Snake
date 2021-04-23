using System;

namespace Commons.Extensions
{
    public static class RandomExtensions
    {
        public static double NextDouble(this Random rand, double min, double max)
        {
            double value = rand.NextDouble();

            return value switch
            {
                <=0 => min,
                >=1 => max,
                _   => value.Lerp(min, max)
            };
        }
    }
}
