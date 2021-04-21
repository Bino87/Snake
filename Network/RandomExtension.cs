using System;

namespace Network
{
    public static class RandomExtension
    {
        public static double NextDouble(this Random rand, double min, double max)
        {
            double t = rand.NextDouble();

            if (t == 0) return min;
            if (t == 1) return max;

            return (1 - t) * min + t * max;
        }
    }
}
