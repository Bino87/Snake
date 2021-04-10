using System;

namespace Network.ActivationFunctions
{
    public class Gausian : IActivationFunction
    {
        public double Evaluate(double value)
        {
            return Math.Pow(Math.E, -Math.Pow(value,2));
        }
    }
}