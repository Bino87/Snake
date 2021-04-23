using System;

namespace Commons.Extensions
{
    public static class ByteExtensions
    {
        public static string ToBinaryString(this byte b) => Convert.ToString(b, 2).PadLeft(8, '0');

    }
}