namespace Commons.Extensions
{
    public static class ObjectExtensions
    {
        public static bool TryParse(this object obj, out int integer)
        {
            if (obj is int i)
            {
                integer = i;
                return true;
            }

            integer = int.MinValue;
            return false;
        }
    }
}
