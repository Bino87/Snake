using Commons.Extensions;

namespace Network.ActivationFunctions
{
    public class EliotSig : IActivationFunction
    {
        public double Evaluate(double value)
        {
            return value / (1 + value.Abs());
        }
    }
}