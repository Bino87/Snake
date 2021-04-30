namespace Network.Mutators
{
    public interface IMutator
    {
        (NetworkData First, NetworkData Second) Get2Offsprings(BasicNeuralNetwork parent1, BasicNeuralNetwork parent2);
    }
}
