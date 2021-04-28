namespace Network.ActivationFunctions
{
    public interface IActivationFunction
    {
        int EvaluationFunctionId { get; }
        double Evaluate(double value);
    }
}
