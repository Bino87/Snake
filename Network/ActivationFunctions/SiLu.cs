using System;
using Network.Enums;

namespace Network.ActivationFunctions
{
    public class SiLu : IActivationFunction
    {
        public ActivationFunctionType EvaluationFunctionId => ActivationFunctionType.SiLu;
        public double Evaluate(double value)
        {
            return value / (1+ Math.Pow(Math.E, -value));
        }
    }
}