using System;
using Network.Enums;

namespace Network.ActivationFunctions
{
    public class Sigmoid : IActivationFunction
    {
        public ActivationFunctionType EvaluationFunctionId => ActivationFunctionType.Sigmoid;
        public double Evaluate(double value)
        {
            return 1d / (1d + Math.Pow(Math.E, -value));
        }
    }
}