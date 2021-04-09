using System;
using System.Collections.Generic;

namespace Network
{
    public class NetworkData
    {
        
        internal double[][] Weights { get; }
        internal  double[][] Bias { get; }

        public NetworkData(double[][] weights, double[][] bias)
        {
            Weights = weights;
            Bias = bias;
        }

        internal byte[] ToByteArr()
        {
            List<byte> bytes = new();

            ConvertToBytes(bytes, Weights);
            ConvertToBytes(bytes, Bias);
           
            return bytes.ToArray();
        }

        private void ConvertToBytes(ICollection<byte> bytes,IReadOnlyList<double[]> arr)
        {
            for(int i = 0; i < arr.Count; i++)
            {
                for(int x = 0; x < arr[i].Length; x++)
                {
                    byte[] b = BitConverter.GetBytes(arr[i][x]);

                    for(int z = 0; z < b.Length; z++)
                    {
                        bytes.Add(b[z]);
                    }
                }
            }
        }

    }
}