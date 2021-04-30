using Network.Enums;
namespace Network.ActivationFunctions
{
    public class BinaryStep : IActivationFunction
    {
        public ActivationFunctionType EvaluationFunctionId => ActivationFunctionType.BinaryStep;

        public double Evaluate(double value)
        {
            return value > 0 ? 1 : 0;
        }
    }
}