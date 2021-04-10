namespace Network.ActivationFunctions
{
    public class BinaryStep : IActivationFunction
    {
        public double Evaluate(double value)
        {
            return value > 0 ? 1 : 0;
        }
    }
}