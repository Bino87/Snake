using System;

namespace Network
{
    public interface IActivationFunction
    {
        double Evaluate(double value);
    }

    public class Input : IActivationFunction
    {
        public double Evaluate(double value)
        {
            return value;
        }
    }
    public class Sigmoid : IActivationFunction
    {
        public double Evaluate(double value)
        {
            return 1d / (1d + Math.Pow(Math.E, -value));
        }
    }

    public class ReLu : IActivationFunction
    {
        public double Evaluate(double value)
        {
            return Math.Max(0, value);
        }
    }
}
