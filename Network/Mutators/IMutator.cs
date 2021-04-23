namespace Network.Mutators
{
    public interface IMutator
    {
        (NetworkInfo First, NetworkInfo Second) Get2Offsprings(NeuralNetwork parent1, NeuralNetwork parent2);
    }
}
