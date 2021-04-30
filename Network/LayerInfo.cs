using Network.ActivationFunctions;
using Network.Enums;

namespace Network
{
    public class LayerInfo
    {
        public ActivationFunctionType ActivationFunction { get; }
        public int NodeCount { get; }


        public LayerInfo(ActivationFunctionType activationFunction, int nodeCount)
        {
            ActivationFunction = activationFunction;
            NodeCount = nodeCount;
        }
    }
}