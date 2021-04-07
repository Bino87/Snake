using System;

namespace Network
{
    public class Network
    {
        private const double cBiasRange = 5;
        private const double cWeightRange = 5;
        private readonly int inputCount;
        private readonly int numLayers;

        readonly double[][] _biases;
        readonly double[][] _weights;
        readonly IActivationFunction _evaluationFunction;

        public Network(IActivationFunction evaluationFunction, params int[] layers)
        {
            if (layers.Length < 2 || evaluationFunction is null)
                throw new Exception();

            inputCount = layers[0];
            numLayers = layers.Length - 1;

            _biases = new double[layers.Length - 1][];
            _weights = new double[layers.Length - 1][];
            _evaluationFunction = evaluationFunction;

            for (int i = 1; i < layers.Length; i++)
            {
                int count = layers[i] * layers[i - 1];
                _weights[i - 1] = new double[count];
                _biases[i - 1] = new double[layers[i]];
            }

            Random rand = new Random();

            for (int i = 0; i < layers.Length - 1; i++)
            {
                for (int x = 0; x < _biases[i].Length; x++)
                {
                    _biases[i][x] = rand.NextDouble(-cBiasRange, cBiasRange);
                }

                for (int x = 0; x < _weights[i].Length; x++)
                {
                    _weights[i][x] = rand.NextDouble(-cWeightRange, cWeightRange);
                }
            }
        }

        public double[] Evaluate(params double[] input)
        {
            if (input.Length != inputCount) throw new Exception();

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

                    output[i] = _evaluationFunction.Evaluate(value + _biases[index][i]) ;
                }


                input = output;
                index++;

            } while (index < numLayers);

            return output;
        }
    }
}
