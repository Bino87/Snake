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

        public static bool IsNull<T>(this T obj) where T : class => obj is null;
        public static bool IsNotNull<T>(this T obj) where T : class => obj is not null;
    }
}
