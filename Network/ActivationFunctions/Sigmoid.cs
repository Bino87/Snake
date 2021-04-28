using System;
using Network.Const;

namespace Network.ActivationFunctions
{
    public class Sigmoid : IActivationFunction
    {
        public int EvaluationFunctionId => ActivationFunctionIds.cSigmoid;
        public double Evaluate(double value)
        {
            return 1d / (1d + Math.Pow(Math.E, -value));
        }
    }
}