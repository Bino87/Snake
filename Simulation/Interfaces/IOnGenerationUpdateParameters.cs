namespace Simulation.Interfaces
{
    public interface IOnGenerationUpdateParameters 
    {
        int Generation { get; set; }
        double AverageFitnessValue { get; set; }
    }
}