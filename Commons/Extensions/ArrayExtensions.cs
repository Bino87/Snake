using System.Collections.Generic;

namespace Commons.Extensions
{
    public static class ArrayExtensions
    {
        public static bool IsEmpty<T>(this ICollection<T> collection)
        {
            return collection.Count == 0;
        }

        public static bool IsNullOrEmpty<T>(this ICollection<T> collection) => collection is null || collection.IsEmpty();
    }
}
