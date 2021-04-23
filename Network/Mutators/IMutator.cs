namespace Network.Mutators
{
    public interface IMutator
    {
        (NetworkInfo First, NetworkInfo Second) GetOffsprings(NeuralNetwork parent1, NeuralNetwork parent2);
    }
}
