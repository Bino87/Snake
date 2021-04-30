using System;
using System.Collections.Generic;
using System.Linq;
using Commons;
using Commons.Extensions;
using Network.Enums;

namespace Network
{

    public class NetworkTemplate
    {
        public int[] BiasCount { get; set; }

        public int[] WeightsCount { get; set; }

        public ActivationFunctionType[] ActivationFunction { get; set; }

        public int Layers { get; set; }

        public int OutputCount { get; set; }

        public int InputCount { get; set; }

        public int[] LayerSetup { get; set; }

        public NetworkTemplate(params LayerInfo[] layerInfos)
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
            ActivationFunction = new ActivationFunctionType[Layers];

            WeightsCount = new int[Layers];
            BiasCount = new int[Layers];

            for (int i = 1; i < layerInfos.Length; i++)
            {
                BiasCount[i - 1] = layerInfos[i].NodeCount;
                WeightsCount[i - 1] = layerInfos[i - 1].NodeCount * layerInfos[i].NodeCount;
                ActivationFunction[i - 1] = layerInfos[i].ActivationFunction;
            }
        }


    }

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