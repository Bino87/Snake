using Network.Const;
namespace Network.ActivationFunctions
{
    public class BinaryStep : IActivationFunction
    {
        public int EvaluationFunctionId => ActivationFunctionIds.cBinaryStep;

        public double Evaluate(double value)
        {
            return value > 0 ? 1 : 0;
        }
    }
}