using System;

namespace Network.ActivationFunctions
{
    public class ReLu : IActivationFunction
    {
        public double Evaluate(double value)
        {
            return Math.Max(0, value);
        }
    }
}