namespace Network.ActivationFunctions
{
    public class LeakyRelu : IActivationFunction
    {
        public double Evaluate(double value)
        {
            if (value > 0)
                return value;
            return value * 0.01d;
        }
    }
}