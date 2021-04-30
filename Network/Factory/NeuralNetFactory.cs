using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Commons;
using Commons.Extensions;
using Network.ActivationFunctions;
using Network.Enums;
using Network.Extensions;

namespace Network.Factory
{
    internal record NeuralNetData(IActivationFunction[] ActivationFunctions, double[][] Weights, double[][] Bias);
    internal static class NeuralNetFactory
    {
        private const double cBiasRange = 10;
        private const double cWeightRange = 1;


        internal static NeuralNetData CreateNew(NetworkTemplate networkData)
        {
            double[][] bias = new double[networkData.Layers][];
            double[][] weights = new double[networkData.Layers][];

            for (int i = 0; i < networkData.Layers; i++)
            {
                bias[i] = new double[networkData.BiasCount[i]];
                weights[i] = new double[networkData.WeightsCount[i]];

                for (int x = 0; x < bias[i].Length; x++)
                {
                    bias[i][x] = RNG.Instance.NextDouble(-cBiasRange, cBiasRange);
                }

                for (int x = 0; x < weights[i].Length; x++)
                {
                    weights[i][x] = RNG.Instance.NextDouble(-cWeightRange, cWeightRange);
                }
            }


            return new NeuralNetData(networkData.ActivationFunction.InitializeFunctions(), weights, bias);
        }

        internal static NetworkData Copy(int inputCount, double[][] biases, double[][] weights,
            ActivationFunctionType[] activationFunctionTypes)
            => new NetworkData(activationFunctionTypes.MakeCopy(), weights.MakeCopy(), biases.MakeCopy(), inputCount,
                biases[^1].Length);
    }
}
