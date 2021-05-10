using System;
using Commons;
using Commons.Extensions;
using DataAccessLibrary.DataTransferObjects.NetworkDTOs;
using DataAccessLibrary.Interfaces;
using Network.Enums;

namespace Network
{
    public class NetworkTemplate : IToMongoDbDataTransferObject<NetworkTemplateDto>
    {
        private const double cBiasRange = 10;
        private const double cWeightRange = 1;

        public Guid Id { get; set; }

        public string Name { get; set; }
        public int[] BiasCount { get; set; }

        public int[] WeightsCount { get; set; }

        public ActivationFunctionType[] ActivationFunction { get; set; }

        public int Layers { get; set; }

        public int OutputCount { get; set; }

        public int InputCount { get; set; }

        public int[] LayerSetup { get; set; }

        public NetworkTemplate(string name, int inputCount, LayerInfo output, params LayerInfo[] hiddenLayers)
        {
            Name = name;

            LayerSetup = new int[hiddenLayers.Length + 2];
            InputCount = inputCount;
            OutputCount = output.NodeCount;
            Layers = hiddenLayers.Length + 1;
            ActivationFunction = new ActivationFunctionType[Layers];
            WeightsCount = new int[Layers];
            BiasCount = new int[Layers];

            LayerSetup[0] = inputCount;
            LayerSetup[^1] = output.NodeCount;

            for (int i = 0; i < hiddenLayers.Length; i++)
            {
                LayerSetup[i + 1] = hiddenLayers[i].NodeCount;
                ActivationFunction[i] = hiddenLayers[i].ActivationFunction;
                BiasCount[i] = hiddenLayers[i].NodeCount;

                WeightsCount[i] = i > 0
                    ? hiddenLayers[i - 1].NodeCount * hiddenLayers[i].NodeCount
                    : inputCount * hiddenLayers[i].NodeCount;
            }

            ActivationFunction[^1] = output.ActivationFunction;
            BiasCount[^1] = output.NodeCount;
            WeightsCount[^1] = (hiddenLayers.Length > 0 ? hiddenLayers[^1].NodeCount : inputCount) * output.NodeCount;
        }




        public NetworkTemplate(NetworkTemplateDto dto)
        {
            Name = dto.Name;
            Layers = dto.Layers;
            WeightsCount = dto.WeightsCount;
            BiasCount = dto.BiasCount;
            ActivationFunction = dto.ActivationFunctions.FromStringArray<ActivationFunctionType>();
            InputCount = dto.InputCount;
            OutputCount = dto.OutputCount;
            LayerSetup = dto.LayerSetup;
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

        public NetworkTemplateDto ToDataTransferObject()
        {
            return new()
            {
                Name = Name,
                Layers = Layers,
                WeightsCount = WeightsCount,
                ActivationFunctions = ActivationFunction.ToStringArray(),
                BiasCount = BiasCount,
                InputCount = InputCount,
                LayerSetup = LayerSetup,
                OutputCount = OutputCount
            };
        }
    }
}