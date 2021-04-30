using System;
using Network.Enums;

namespace Network.ActivationFunctions
{
    public class SoftPlus : IActivationFunction
    {
        public ActivationFunctionType EvaluationFunctionId => ActivationFunctionType.SoftPlus;
        public double Evaluate(double value)
        {
            return Math.Log(1 + Math.Pow(Math.E, value));
        }
    }
}