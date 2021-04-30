using Network.Enums;

namespace Network.ActivationFunctions
{
    public interface IActivationFunction
    {
        ActivationFunctionType EvaluationFunctionId { get; }
        double Evaluate(double value);
    }
}
