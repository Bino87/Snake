using System.Collections.Generic;
using System.Threading;
using Commons.Extensions;
using DataAccessLibrary.DataAccessors.Network;
using DataAccessLibrary.DataTransferObjects.NetworkDTOs;
using Network.Enums;

namespace Network
{
    public class NetworkData 
    {
        public double[][] Weights { get; private set; }
        public double[][] Bias { get; private set; }
        public int OutputCount { get; set; }
        public int InputCount { get; set; }
        public ActivationFunctionType[] ActivationFunction { get; set; }
        public int Layers { get; set; }


        public NetworkData(ActivationFunctionType[] activationFunction, double[][] weights, double[][] bias, int inputCount, int outputCount)
        {
            Weights = weights;
            Bias = bias;
            Layers = weights.Length;
            ActivationFunction = activationFunction;

            InputCount = inputCount;
            OutputCount = outputCount;
            SaveToDB();
        }

        void SaveToDB()
        {

            NetworkWeightAccess nwa = new NetworkWeightAccess();
            List<NetworkWeightDto> dtos = new List<NetworkWeightDto>();

            for (int i = 0; i < Weights.Length; i++)
            {
                var item = Weights[i];

                for (int j = 0; j < item.Length; j++)
                {
                    NetworkWeightDto dto = new(i, item[j], j);
                    dtos.Add(dto);
                }
            }

            var arrIn = dtos.ToArray();
            nwa.InsertMany(arrIn);

        }


        internal byte[] ToByteArr()
        {
            int weightCount = Weights.TotalLength() * 8;
            int biasCount = Bias.TotalLength()* 8;

            byte[] arr = new byte[weightCount + biasCount];

            ConvertToBytes(arr, Weights, 0);
            ConvertToBytes(arr, Bias, weightCount);

            return arr;
        }


        static void ConvertToBytes(byte[] arr, double[][] values, int index)
        {
            for (int i = 0; i < values.Length; i++)
            {
                for (int x = 0; x < values[i].Length; x++)
                {
                    byte[] b = values[i][x].GetBytes();

                    for (int z = 0; z < b.Length; z++)
                    {
                        arr[index++] = b[z];
                    }
                }
            }
        }

        internal void FromByteArray(byte[] arr)
        {
            int index = 0;
            Weights = Create2DArrayFromBytes(arr, ref index, Weights.GetCounts(), true);
            Bias = Create2DArrayFromBytes(arr, ref index, Bias.GetCounts(), false);
        }

        private double[][] Create2DArrayFromBytes(byte[] arr, ref int index, IReadOnlyList<int> lookUp, bool clamp)
        {
            double[][] item = new double[lookUp.Count][];

            for (int i = 0; i < Layers; i++)
            {
                item[i] = new double[lookUp[i]];

                for (int w = 0; w < lookUp[i]; w++)
                {
                    double d = clamp ? arr.ToDouble(index).Clamp1Neg1() : arr.ToDouble(index);

                    item[i][w] = d;

                    index += 8;
                }
            }

            return item;
        }
    }
}