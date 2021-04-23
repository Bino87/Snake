using System;
using System.Collections.Generic;
using System.Linq;
using Commons;
using Commons.Extensions;
using Network.ActivationFunctions;

namespace Network
{
    public class NetworkInfo
    {
        private const double cBiasRange = 10;
        private const double cWeightRange = 1;

        public double[][] Weights { get; private set; }
        public double[][] Bias { get; private set; }

        public int InputCount { get; }
        public int OutputCount { get; }
        public int Layers { get; }
        internal int[] WeightsCount { get; }
        internal int[] BiasCount { get; }
        public int[] LayerSetup { get; }
        internal IActivationFunction[] ActivationFunction { get; }


        public NetworkInfo(params LayerInfo[] layerInfos)
        {
            if (layerInfos.Length < 2)
                throw new Exception();

            LayerSetup = new int[layerInfos.Length];

            for (int i = 0; i < layerInfos.Length; i++)
            {
                LayerSetup[i] = layerInfos[i].NodeCount;
            }

            InputCount = layerInfos[0].NodeCount;
            OutputCount = layerInfos[^1].NodeCount;
            Layers = layerInfos.Length - 1;
            ActivationFunction = new IActivationFunction[Layers];

            WeightsCount = new int[Layers];
            BiasCount = new int[Layers];

            for (int i = 1; i < layerInfos.Length; i++)
            {
                BiasCount[i - 1] = layerInfos[i].NodeCount;
                WeightsCount[i - 1] = layerInfos[i - 1].NodeCount * layerInfos[i].NodeCount;
                ActivationFunction[i - 1] = layerInfos[i].ActivationFunction;
            }
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

        protected NetworkInfo(int[] biasCount, int[] weightCount, IActivationFunction[] activationFunction, int inputCount, int outputCount, int layers)
        {
            Layers = layers;
            WeightsCount = weightCount;
            BiasCount = biasCount;
            ActivationFunction = activationFunction;

            InputCount = inputCount;
            OutputCount = outputCount;
        }

        public NetworkInfo Copy()
        {
            return new(BiasCount, WeightsCount, ActivationFunction, InputCount, OutputCount, Layers);
        }


        public (double[][] Weights, double[][] Biass) GetWeightsAndBiases()
        {
            if (Weights is not null && Bias is not null)
                return (Weights, Bias);
            return (Weights, Bias) = InitiateRandomWeightsAndBiases();
        }

        private (double[][] Weights, double[][] Biass) InitiateRandomWeightsAndBiases()
        {
            double[][] bias = new double[Layers][];
            double[][] weights = new double[Layers][];

            for (int i = 0; i < BiasCount.Length; i++)
            {
                bias[i] = new double[BiasCount[i]];
            }

            for (int i = 0; i < WeightsCount.Length; i++)
            {
                weights[i] = new double[WeightsCount[i]];
            }


            for (int i = 0; i < Layers; i++)
            {
                for (int x = 0; x < bias[i].Length; x++)
                {
                    bias[i][x] = RNG.Instance.NextDouble(-cBiasRange, cBiasRange);
                }

                for (int x = 0; x < weights[i].Length; x++)
                {
                    weights[i][x] = RNG.Instance.NextDouble(-cWeightRange, cWeightRange);
                }
            }

            return (weights, bias);
        }

        internal byte[] ToByteArr()
        {
            int weightCount = Weights.Sum(t => t.Length * 8);
            int biasCount = Bias.Sum(t => t.Length * 8);

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
            Weights = Create2DArrayFromBytes(arr, ref index, WeightsCount, true);
            Bias = Create2DArrayFromBytes(arr, ref index, BiasCount, false);
        }

        private double[][] Create2DArrayFromBytes(byte[] arr, ref int index, IReadOnlyList<int> lookUp, bool clamp)
        {
            double[][] item = new double[lookUp.Count][];

            for (int i = 0; i < lookUp.Count; i++)
            {
                item[i] = new double[lookUp[i]];
            }

            for (int i = 0; i < Layers; i++)
            {
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