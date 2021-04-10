namespace Network.ActivationFunctions
{
    public class Identity : IActivationFunction
    {
        public double Evaluate(double value)
        {
            return value;
        }
    }
}