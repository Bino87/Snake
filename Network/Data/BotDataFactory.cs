using System;
using System.Collections.Generic;
using Commons.Extensions;
using DataAccessLibrary.DataAccessors.Network;
using DataAccessLibrary.DataTransferObjects.NetworkDTOs;
using Network.ActivationFunctions;

namespace Network.Data
{
    public class BotDataFactory
    {
        private record BotData(int Id, IActivationFunction[] ActivationFunctions, double[][] Bias, double[][] Weights);

        private static List<BotData> _botData = new();
        public static void SaveBots()
        {
            List<NetworkLayerDto> networkLayers = new();
            List<NetworkBiasDto> networkBias = new();
            List<NetworkWeightDto> networkWeights = new();

            foreach (BotData botData in _botData)
            {
                for (int i = 0; i < botData.ActivationFunctions.Length; i++)
                {
                    int activationFunctionId = (int)botData.ActivationFunctions[i].EvaluationFunctionId;
                    int count = botData.Bias[i].Length;

                    NetworkLayerDto layerDto = new NetworkLayerDto(botData.Id, activationFunctionId, count, i);
                    networkLayers.Add(layerDto);


                }
            }

            networkLayers = new List<NetworkLayerDto>(
                new NetworkLayerAccess().InsertMany(networkLayers.ToArray())
                );

            for (int index = 0; index < _botData.Count; index++)
            {
                BotData botData = _botData[index];
                for (int i = 0; i < botData.Bias.Length; i++)
                {
                    int layerId = networkLayers[index * botData.Bias.Length + i].Id;//assumes that all networks have the same number of layers

                    for (int b = 0; b < botData.Bias[i].Length; b++)
                    {
                        NetworkBiasDto biasDto = new()
                        {
                            InternalIndex = b,
                            LayerId = layerId,
                            Value = botData.Bias[i][b]
                        };

                        networkBias.Add(biasDto);
                    }

                    for (int w = 0; w < botData.Weights[i].Length; w++)
                    {
                        NetworkWeightDto weightDto = new NetworkWeightDto(layerId, botData.Weights[i][w], w);
                        networkWeights.Add(weightDto);
                    }
                }
            }

            new NetworkWeightAccess().InsertMany(networkWeights.ToArray());
            new NetworkBiasAccess().InsertMany(networkBias.ToArray());

            _botData.Clear();
        }

        public static void Register(int id, IActivationFunction[] activationFunction, double[][] biases, double[][] weights)
        {
            _botData.Add(new BotData(id, activationFunction, biases, weights));
        }
    }
}
