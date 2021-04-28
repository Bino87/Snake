using Commons.Extensions;
using Network.Const;

namespace Network.ActivationFunctions
{
    public class EliotSig : IActivationFunction
    {
        public int EvaluationFunctionId => ActivationFunctionIds.cEliotSig;
        public double Evaluate(double value)
        {
            return value / (1 + value.Abs());
        }
    }
}