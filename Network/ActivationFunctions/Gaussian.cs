using System;
using Commons.Extensions;
using Network.Enums;

namespace Network.ActivationFunctions
{
    public class Gaussian : IActivationFunction
    {
        public ActivationFunctionType EvaluationFunctionId => ActivationFunctionType.Gaussian;
        public double Evaluate(double value)
        {
            return Math.Pow(Math.E, -value.Pow2());
        }
    }
}