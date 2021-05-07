using System;
using Commons;
using Network.Enums;

namespace Network
{
    public class NetworkTemplate
    {
        private const double cBiasRange = 10;
        private const double cWeightRange = 1;

        public int[] BiasCount { get; set; }

        public int[] WeightsCount { get; set; }

        public ActivationFunctionType[] ActivationFunction { get; set; }

        public int Layers { get; set; }

        public int OutputCount { get; set; }

        public int InputCount { get; set; }

        public int[] LayerSetup { get; set; }

        public NetworkTemplate(int inputCount, LayerInfo output, params LayerInfo[] hiddenLayers)
        {
            LayerSetup = new int[hiddenLayers.Length + 2];
            InputCount = inputCount;
            OutputCount = output.NodeCount;
            Layers = hiddenLayers.Length + 1;

            LayerSetup[0] = inputCount;
            LayerSetup[^1] = output.NodeCount;

            for (int i = 0; i < hiddenLayers.Length; i++)
            {
                LayerSetup[i + 1] = hiddenLayers[i].NodeCount;
            }

            ActivationFunction = new ActivationFunctionType[Layers];
            WeightsCount = new int[Layers];
            BiasCount = new int[Layers];

            for (int i = 0; i < hiddenLayers.Length; i++)
            {
                ActivationFunction[i] = hiddenLayers[i].ActivationFunction;
                BiasCount[i] = hiddenLayers[i].NodeCount;

                WeightsCount[i] = i > 0 ? hiddenLayers[i - 1].NodeCount * hiddenLayers[i].NodeCount : inputCount * hiddenLayers[i].NodeCount;
            }

            ActivationFunction[^1] = output.ActivationFunction;
            BiasCount[^1] = output.NodeCount;
            WeightsCount[^1] = (hiddenLayers.Length > 0 ? hiddenLayers[^1].NodeCount : inputCount) * output.NodeCount;
        }

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

        public NetworkData ToNetworkData()
        {
            double[][] bias = new double[Layers][];
            double[][] weights = new double[Layers][];

            for (int i = 0; i < Layers; i++)
            {
                bias[i] = new double[BiasCount[i]];
                weights[i] = new double[WeightsCount[i]];

                for (int x = 0; x < bias[i].Length; x++)
                {
                    bias[i][x] = RNG.Instance.NextDouble(-cBiasRange, cBiasRange);
                }

                for (int x = 0; x < weights[i].Length; x++)
                {
                    weights[i][x] = RNG.Instance.NextDouble(-cWeightRange, cWeightRange);
                }
            }


            return new NetworkData(ActivationFunction, weights, bias, InputCount, OutputCount);
        }
    }
}