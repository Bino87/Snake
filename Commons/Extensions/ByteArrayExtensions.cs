
using System;
using System.Text;

namespace Commons.Extensions
{
    public static class ByteArrayExtensions
    {
        public static string ToBinaryString(this byte[] arr)
        {
            StringBuilder sb = new();

            for (int i = 0; i < arr.Length; i++)
            {
                string str = arr[i].ToBinaryString();

                sb.Append(str);
            }

            return sb.ToString();
        }

        public static double ToDouble(this byte[] arr, int index) => BitConverter.ToDouble(arr, index);
        public static int ToInt(this byte[] arr, int index) => BitConverter.ToInt32(arr, index);
        public static float ToFloat(this byte[] arr, int index) => BitConverter.ToSingle(arr, index);
    }
}