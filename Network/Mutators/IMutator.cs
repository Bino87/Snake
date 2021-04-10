namespace Network.Mutators
{
    public interface IMutator
    {
        (NeuralNetwork First, NeuralNetwork Second) GetOffsprings(NeuralNetwork father, NeuralNetwork mother);
    }
}
