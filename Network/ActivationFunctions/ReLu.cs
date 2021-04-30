using System;
using Network.Enums;

namespace Network.ActivationFunctions
{
    public class ReLu : IActivationFunction
    {
        public ActivationFunctionType EvaluationFunctionId => ActivationFunctionType.ReLu;
        public double Evaluate(double value)
        {
            return Math.Max(0, value);
        }
    }
}