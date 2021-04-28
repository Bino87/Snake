using System;
using Network.Const;

namespace Network.ActivationFunctions
{
    public class SiLu : IActivationFunction
    {
        public int EvaluationFunctionId => ActivationFunctionIds.cSiLu;
        public double Evaluate(double value)
        {
            return value / (1+ Math.Pow(Math.E, -value));
        }
    }
}