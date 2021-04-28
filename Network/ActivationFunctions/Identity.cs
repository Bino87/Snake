using Network.Const;

namespace Network.ActivationFunctions
{
    public class Identity : IActivationFunction
    {
        public int EvaluationFunctionId => ActivationFunctionIds.cIdentity;
        public double Evaluate(double value)
        {
            return value;
        }
    }
}