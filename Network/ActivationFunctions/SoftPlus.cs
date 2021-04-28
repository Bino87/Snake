using System;
using Network.Const;

namespace Network.ActivationFunctions
{
    public class SoftPlus : IActivationFunction
    {
        public int EvaluationFunctionId => ActivationFunctionIds.cSoftPlus;
        public double Evaluate(double value)
        {
            return Math.Log(1 + Math.Pow(Math.E, value));
        }
    }
}