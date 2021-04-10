using System;

namespace Network.ActivationFunctions
{
    public class SiLu : IActivationFunction
    {
        public double Evaluate(double value)
        {
            return value / (1+ Math.Pow(Math.E, -value));
        }
    }
}