using Commons.Extensions;
using Network.Enums;

namespace Network.ActivationFunctions
{
    public class EliotSig : IActivationFunction
    {
        public ActivationFunctionType EvaluationFunctionId => ActivationFunctionType.EliotSig;
        public double Evaluate(double value)
        {
            return value / (1 + value.Abs());
        }
    }
}