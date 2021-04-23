using System;
using Commons.Extensions;

namespace Commons
{
    public class RNG
    {
        private readonly Random _rand;
        private readonly object _key = new();

        private static RNG _instance;

        public static RNG Instance => _instance ??= new RNG();


        public RNG()
        {
            _rand = new Random(13 * 11);
        }

        public int Next(int min, int max)
        {
            lock (_key)
            {
                return _rand.Next(min, max);
            }
        }

        public int Next(int max) => Next(0, max);

        public double NextDouble() => NextDouble(0, 1);

        public double NextDouble(double min, double max)
        {
            lock (_key)
            {
                return _rand.NextDouble(min, max);
            }
        }

        public double NextDouble(double max) => NextDouble(0, max);

    }
}
