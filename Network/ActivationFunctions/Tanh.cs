using System;
using Network.Enums;

namespace Network.ActivationFunctions
{
    public class Tanh : IActivationFunction
    {
        public ActivationFunctionType EvaluationFunctionId => ActivationFunctionType.TanH;
        public double Evaluate(double value)
        {
            double eToMinusX = Math.Pow(Math.E, -value);
            double eToX = Math.Pow(Math.E, value);
            return  (eToX - eToMinusX) / (eToX + eToMinusX);
        }
    }
}