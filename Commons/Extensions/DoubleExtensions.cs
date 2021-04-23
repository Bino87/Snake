namespace Commons.Extensions
{
    public static class DoubleExtensions
    {
        public static double Lerp(this double value, double min, double max)
        {
            return min * (1 - value) + max * value;
        }
    }
}