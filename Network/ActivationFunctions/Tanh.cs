using System;
using Network.Const;

namespace Network.ActivationFunctions
{
    public class Tanh : IActivationFunction
    {
        public int EvaluationFunctionId => ActivationFunctionIds.cTanH;
        public double Evaluate(double value)
        {
            double eToMinusX = Math.Pow(Math.E, -value);
            double eToX = Math.Pow(Math.E, value);
            return  (eToX - eToMinusX) / (eToX + eToMinusX);
        }
    }
}