namespace Network.Mutators
{
    public interface IMutator
    {
        (NetworkInfo First, NetworkInfo Second) Get2Offsprings(BasicNeuralNetwork parent1, BasicNeuralNetwork parent2);
    }
}
