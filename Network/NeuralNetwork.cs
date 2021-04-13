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

                double[] input1 = input;
                int index = layerIndex;
                double[] output1 = output;
                Parallel.For(0, output.Length,
                             (outputIndex) =>
                             {
                                 double value = 0;


                                 for (int inputIndex = 0; inputIndex < input1.Length; inputIndex++)
                                 {
                                     int weightIndex = outputIndex * input1.Length + inputIndex;
                                     double result = input1[inputIndex] * _weights[index][weightIndex];
                                     if (double.IsNaN(result) || double.IsInfinity(result))
                                     {
                                         continue;
                                     }

                                     value += result;
                                 }

                                 double res = _activationFunction[index].Evaluate(value + _biases[index][outputIndex]);
                                 if (double.IsInfinity(res) || double.IsNaN(res)) res = 0;
                                 output1[outputIndex] = res;
                             }
                    );

                input = output;
                layerIndex++;

            } while (layerIndex < _numLayers);

            return output;
        }
    }
}
