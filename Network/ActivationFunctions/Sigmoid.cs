using System;

namespace Network.ActivationFunctions
{
    public class Sigmoid : IActivationFunction
    {
        public double Evaluate(double value)
        {
            return 1d / (1d + Math.Pow(Math.E, -value));
        }
    }
}