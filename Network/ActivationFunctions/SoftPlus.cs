using System;

namespace Network.ActivationFunctions
{
    public class SoftPlus : IActivationFunction
    {
        public double Evaluate(double value)
        {
            return Math.Log(1 + Math.Pow(Math.E, value));
        }
    }
}