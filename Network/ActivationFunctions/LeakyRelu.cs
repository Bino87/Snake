using Network.Enums;

namespace Network.ActivationFunctions
{
    public class LeakyRelu : IActivationFunction
    {
        public ActivationFunctionType EvaluationFunctionId => ActivationFunctionType.LeakyReLu;
        public double Evaluate(double value)
        {
            if (value > 0)
                return value;
            return value * 0.01d;
        }
    }
}