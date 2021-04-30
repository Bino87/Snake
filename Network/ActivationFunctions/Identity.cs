using Network.Enums;

namespace Network.ActivationFunctions
{
    public class Identity : IActivationFunction
    {
        public ActivationFunctionType EvaluationFunctionId => ActivationFunctionType.Identity;
        public double Evaluate(double value)
        {
            return value;
        }
    }
}