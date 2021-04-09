namespace Network
{
    public class LayerInfo
    {
        public IActivationFunction ActivationFunction { get; }
        public int NodeCount { get; }


        public LayerInfo(IActivationFunction activationFunction, int nodeCount)
        {
            ActivationFunction = activationFunction;
            NodeCount = nodeCount;
        }

        
    }
}