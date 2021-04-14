﻿using System;
using System.Collections.Generic;
using Network.ActivationFunctions;

namespace Network
{
    public class NetworkInfo
    {
        private const double cBiasRange = 1;
        private const double cWeightRange = 1;

        public double[][] Weights { get; private set; }
        public double[][] Bias { get; private set; }

        public bool HasValues => Weights is not null && Bias is not null;
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

        protected NetworkInfo(int[] biasCount,int[] weightCount, IActivationFunction[] activationFunction, int inputCount, int outputCount, int layers)
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
            return new(BiasCount, WeightsCount,ActivationFunction, InputCount, OutputCount, Layers);
        }


        public (double[][] Weights, double[][] Biass) CreateWeightsAndBiases()
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

            Random rand = new();

            for (int i = 0; i < Layers; i++)
            {
                for (int x = 0; x < bias[i].Length; x++)
                {
                    bias[i][x] = rand.NextDouble(-cBiasRange, cBiasRange);
                }

                for (int x = 0; x < weights[i].Length; x++)
                {
                    weights[i][x] = rand.NextDouble(-cWeightRange, cWeightRange);
                }
            }

            return (weights, bias);
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
            Weights = Create2DArrayFromBytes(arr, ref index, WeightsCount, true);
            Bias = Create2DArrayFromBytes(arr, ref index, BiasCount,  false);
        }

        double[][] Create2DArrayFromBytes(byte[] arr, ref int index, int[] lookUp,  bool clamp)
        {
            double[][] item = new double[lookUp.Length][];

            for(int i = 0; i < lookUp.Length; i++)
            {
                item[i] = new double[lookUp[i]];
            }

            for (int i = 0; i < Layers; i++)
            {
                for (int w = 0; w < lookUp[i]; w++)
                {
                    item[i][w] = clamp ? Clamp(BitConverter.ToDouble(arr, index)) : BitConverter.ToDouble(arr, index);

                    index += 8;
                }
            }

            return item;
        }

        double Clamp(double d)
        {
            return d switch
            {
                > 1 => 1,
                < -1 => -1,
                _ => d
            };
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