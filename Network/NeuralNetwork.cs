using System;
using System.Threading.Tasks;
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

        public NeuralNetwork(NetworkInfo networkInfo)
        {
            _biases = networkInfo.Bias;
            _weights = networkInfo.Weights;
            _numLayers = networkInfo.Layers;
            _inputCount = networkInfo.InputCount;
            _activationFunction = networkInfo.ActivationFunction;
        }

        public NetworkInfo ToNetworkInfo() => new(_activationFunction, _weights, _biases, _inputCount, _biases[^1].Length);

        public double[] Evaluate(double[] input)
        {
            if (input.Length != _inputCount) throw new Exception();

            double[] output;
            int layerIndex = 0;

            do
            {
                output = new double[_biases[layerIndex].Length];

                Parallel.For(0, output.Length,
                             (outputIndex) =>
                             {
                                 double value = 0;


                                 for (int inputIndex = 0; inputIndex < input.Length; inputIndex++)
                                 {
                                     int weightIndex = outputIndex * input.Length + inputIndex;
                                     value += input[inputIndex] * _weights[layerIndex][weightIndex];
                                 }

                                 output[outputIndex] = _activationFunction[layerIndex].Evaluate(value + _biases[layerIndex][outputIndex]);
                             }
                    );

                input = output;
                layerIndex++;

            } while (layerIndex < _numLayers);

            return output;
        }
    }
}
