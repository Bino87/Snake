using System;
using Network.Const;

namespace Network.ActivationFunctions
{
    public class ReLu : IActivationFunction
    {
        public int EvaluationFunctionId => ActivationFunctionIds.cReLu;
        public double Evaluate(double value)
        {
            return Math.Max(0, value);
        }
    }
}