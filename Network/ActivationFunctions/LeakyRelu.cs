using Network.Const;

namespace Network.ActivationFunctions
{
    public class LeakyRelu : IActivationFunction
    {
        public int EvaluationFunctionId => ActivationFunctionIds.cLeakyReLu;
        public double Evaluate(double value)
        {
            if (value > 0)
                return value;
            return value * 0.01d;
        }
    }
}