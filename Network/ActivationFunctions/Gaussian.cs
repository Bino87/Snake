using System;
using Commons.Extensions;
using Network.Const;

namespace Network.ActivationFunctions
{
    public class Gaussian : IActivationFunction
    {
        public int EvaluationFunctionId => ActivationFunctionIds.cGaussian;
        public double Evaluate(double value)
        {
            return Math.Pow(Math.E, -value.Pow2());
        }
    }
}