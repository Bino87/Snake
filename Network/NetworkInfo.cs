using System;
using System.Collections.Generic;
using Network.ActivationFunctions;

namespace Network
{
    public class NetworkInfo
    {
        private const double cBiasRange = 5;
        private const double cWeightRange = 1;

        internal double[][] Weights { get; }
        internal double[][] Bias { get; }
        public int InputCount { get; }
        public int OutputCount { get; }
        internal int Layers { get; }
        internal int[] WeightsCount { get; }
        internal int[] BiasCount { get; }
        internal IActivationFunction[] ActivationFunction { get; }


        public NetworkInfo(params LayerInfo[] layerInfos)
        {
            if (layerInfos.Length < 2)
                throw new Exception();

            InputCount = layerInfos[0].NodeCount;
            OutputCount = layerInfos[^1].NodeCount;
            Layers = layerInfos.Length - 1;
            Bias = new double[Layers][];
            Weights = new double[Layers][];
            ActivationFunction = new IActivationFunction[Layers];

            WeightsCount = new int[Layers];
            BiasCount = new int[Layers];

            for (int i = 1; i < layerInfos.Length; i++)
            {
                ActivationFunction[i - 1] = layerInfos[i].ActivationFunction;
            }

            for (int i = 1; i < layerInfos.Length; i++)
            {
                int count = layerInfos[i].NodeCount * layerInfos[i - 1].NodeCount;
                Weights[i - 1] = new double[count];
                Bias[i - 1] = new double[layerInfos[i].NodeCount];

            }

            CreateWeightsAndBiases();
        }

        public NetworkInfo(IActivationFunction[] activationFunction, double[][] weights, double[][] bias, int inputCount, int outputCount)
        {
            Weights = weights;
            Bias = bias;
            Layers = weights.Length;
            WeightsCount = new int[Layers];
            BiasCount = new int[Layers];
            ActivationFunction = activationFunction;

            InputCount = inputCount;
            OutputCount = outputCount;

            for (int i = 0; i < Layers; i++)
            {
                WeightsCount[i] = weights[i].Length;
                BiasCount[i] = bias[i].Length;
            }
        }

        private void CreateWeightsAndBiases()
        {
            Random rand = new();

            for (int i = 0; i < Layers; i++)
            {
                WeightsCount[i] = Weights[i].Length;
                BiasCount[i] = Bias[i].Length;

                for (int x = 0; x < Bias[i].Length; x++)
                {
                    Bias[i][x] = rand.NextDouble(-cBiasRange, cBiasRange);
                }

                for (int x = 0; x < Weights[i].Length; x++)
                {
                    Weights[i][x] = rand.NextDouble(-cWeightRange, cWeightRange);
                }
            }
        }

        internal byte[] ToByteArr()
        {
            List<byte> bytes = new();

            ConvertToBytes(bytes, Weights);
            ConvertToBytes(bytes, Bias);

            return bytes.ToArray();
        }

        internal void FromByteArray(byte[] arr)
        {
            int index = 0;
            Create2DArrayFromBytes(arr, ref index, WeightsCount, Weights);
            Create2DArrayFromBytes(arr, ref index, BiasCount, Bias);
        }

        void Create2DArrayFromBytes(byte[] arr, ref int index, int[] lookUp, double[][] item)
        {

            for (int i = 0; i < Layers; i++)
            {
                for (int w = 0; w < lookUp[i]; w++)
                {
                    item[i][w] = BitConverter.ToDouble(arr, index);
                    index += 8;
                }
            }
        }

        private void ConvertToBytes(ICollection<byte> bytes, IReadOnlyList<double[]> arr)
        {
            for (int i = 0; i < Layers; i++)
            {
                for (int x = 0; x < arr[i].Length; x++)
                {
                    byte[] b = BitConverter.GetBytes(arr[i][x]);

                    for (int z = 0; z < b.Length; z++)
                    {
                        bytes.Add(b[z]);
                    }
                }
            }
        }
    }
}