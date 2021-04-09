using System;

namespace Network
{
    public class NeuralNetwork
    {
        private readonly int _inputCount;
        private readonly int _numLayers;

        readonly double[][] _biases;
        readonly double[][] _weights;
        readonly IActivationFunction[] _activationFuntionFunction;

        public NeuralNetwork(NetworkInfo networkInfo)
        {
            _biases = networkInfo.Bias;
            _weights = networkInfo.Weights;
            _numLayers = networkInfo.Layers;
            _inputCount = networkInfo.InputCount;
            _activationFuntionFunction = networkInfo.ActivationFunction;
        }

        internal NetworkInfo ToNetworkData() => new(_activationFuntionFunction, _weights, _biases);

        public double[] Evaluate(params double[] input)
        {
            if (input.Length != _inputCount) throw new Exception();

            double[] output;
            int index = 0;

            do
            {
                output = new double[_biases[index].Length];

                for (int i = 0; i < output.Length; i++)
                {
                    double value = 0;
                    for (int x = 0; x < input.Length; x++)
                    {
                        value += input[i] * _weights[index][i * output.Length + x];
                    }

                    output[i] = _activationFuntionFunction[index].Evaluate(value + _biases[index][i]) ;
                }


                input = output;
                index++;

            } while (index < _numLayers);

            return output;
        }
    }
}
