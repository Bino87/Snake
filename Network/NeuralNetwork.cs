using System;
using Network.ActivationFunctions;

namespace Network
{
    public class NeuralNetwork
    {
        private readonly int _inputCount;
        private readonly int _numLayers;

        readonly double[][] _biases;
        readonly double[][] _weights;
        readonly IActivationFunction[] _activationFunction;

        public double[][] Weights => _weights;

        public NeuralNetwork(NetworkInfo networkInfo)
        {
            if (networkInfo.HasValues)
            {
                _biases = networkInfo.Bias;
                _weights = networkInfo.Weights;
            }
            else
            {
                (double[][] weights, double[][] biass) = networkInfo.CreateWeightsAndBiases();
                _biases = biass;
                _weights = weights;
            }
            
            _numLayers = networkInfo.Layers;
            _inputCount = networkInfo.InputCount;
            _activationFunction = networkInfo.ActivationFunction;
        }

        public NetworkInfo ToNetworkInfo() => new(_activationFunction, _weights, _biases, _inputCount, _biases[^1].Length);

        public double[][] Evaluate(double[] input)
        {
            if (input.Length != _inputCount) throw new Exception();

            double[][] output = new double[1 + _numLayers][];
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

            } while (layerIndex < _numLayers);

            return output;
        }

        private double CalculateValue(double[] input, int outputIndex, int index)
        {

            double value = 0;
            for(int inputIndex = 0; inputIndex < input.Length; inputIndex++)
            {
                int weightIndex = outputIndex * input.Length + inputIndex;
                double result = input[inputIndex] * _weights[index][weightIndex];
                if (double.IsNaN(result) || double.IsInfinity(result))
                {
                    continue;
                }

                value += result;
            }

            return value;
        }
    }
}
