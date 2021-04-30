using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Commons.Extensions
{
    public static class ArrayExtensions
    {
        public static bool IsEmpty<T>(this ICollection<T> collection)
        {
            return collection.Count == 0;
        }

        public static int TotalLength<T>(this T[][] arr)
        {
            return arr.Sum(t => t.Length);
        }

        public static int[] GetCounts<T>(this T[][] arr)
        {
            int[] res = new int[arr.Length];

            for (int i = 0; i < res.Length; i++)
            {
                res[i] = arr[i].Length;
            }

            return res;
        }

        public static T[][] MakeCopy<T>(this T[][] arr) where T : unmanaged
        {
            T[][] nArr = new T[arr.Length][];

            for (int i = 0; i < nArr.Length; i++)
            {
                nArr[i] = arr[i].MakeCopy();
            }

            return nArr;
        }
        public static T[] MakeCopy<T>(this T[] arr) where T : unmanaged
        {
            T[] nArr = new T[arr.Length];

            for (int i = 0; i < arr.Length; i++)
            {
                nArr[i] = arr[i];
            }

            return nArr;
        }

        public static bool IsNullOrEmpty<T>(this ICollection<T> collection) => collection is null || collection.IsEmpty();
    }
}
