using System;
using Commons.Extensions;
using Network.ActivationFunctions;
using Network.Extensions;
using Network.Factory;

namespace Network
{
    public class BasicNeuralNetwork
    {
        private readonly int _inputCount;
        private readonly int _layerCount;

        private readonly double[][] _biases;
        private readonly double[][] _weights;
        private readonly IActivationFunction[] _activationFunction;

        public double[][] Weights => _weights;

        public BasicNeuralNetwork(NetworkTemplate networkTemplate) : this(networkTemplate.Layers, networkTemplate.InputCount)
        {
            (IActivationFunction[] activationFunctions, double[][] weights, double[][] bias) = NeuralNetFactory.CreateNew(networkTemplate);
            _weights = weights;
            _biases = bias;
            _activationFunction = activationFunctions;
        }

        private BasicNeuralNetwork(int layerCount, int inputCount)
        {
            _layerCount = layerCount;
            _inputCount = inputCount;
        }

        public BasicNeuralNetwork(NetworkData networkData) : this(networkData.Layers, networkData.InputCount)
        {
            _weights = networkData.Weights;
            _biases = networkData.Bias;
            _activationFunction = networkData.ActivationFunction.InitializeFunctions();
        }

        public NetworkData CopyNetworkInfo() => NeuralNetFactory.Copy(_inputCount, _biases, _weights, _activationFunction.ToActivationFunctionTypes());

        public double[][] Evaluate(double[] input)
        {
            if (input.Length != _inputCount) throw new Exception();

            double[][] output = new double[1 + _layerCount][];
            output[0] = input;
            int layerIndex = 0;

            do
            {
                output[layerIndex + 1] = new double[_biases[layerIndex].Length];

                for (int outputIndex = 0; outputIndex < output[layerIndex + 1].Length; outputIndex++)
                {
                    double value = CalculateValue(input, outputIndex, layerIndex);

                    double vBias = value + _biases[layerIndex][outputIndex];
                    double res = _activationFunction[layerIndex].Evaluate(vBias);
                    if (double.IsInfinity(res) || double.IsNaN(res)) res = 0;
                    output[layerIndex + 1][outputIndex] = res;
                }

                input = output[layerIndex + 1];
                layerIndex++;

            } while (layerIndex < _layerCount);

            return output;
        }

        private double CalculateValue(double[] input, int outputIndex, int index)
        {
            double value = 0;

            for (int inputIndex = 0; inputIndex < input.Length; inputIndex++)
            {
                int weightIndex = outputIndex * input.Length + inputIndex;
                double result = input[inputIndex] * _weights[index][weightIndex];

                if (result.IsOkNumber())
                    value += result;
            }

            return value;
        }
    }
}
