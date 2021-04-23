using System;
using Commons.Extensions;

namespace Network.ActivationFunctions
{
    public class Gaussian : IActivationFunction
    {
        public double Evaluate(double value)
        {
            return Math.Pow(Math.E, -value.Pow2());
        }
    }
}