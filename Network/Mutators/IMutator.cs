namespace Network.Mutators
{
    public interface IMutator
    {
        (NetworkInfo First, NetworkInfo Second) GetOffsprings(NeuralNetwork father, NeuralNetwork mother);
    }
}
