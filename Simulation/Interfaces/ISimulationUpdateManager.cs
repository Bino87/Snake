namespace Simulation.Interfaces
{
    public interface ISimulationUpdateManager
    {
        IUpdate<IOnGenerationUpdateParameters> OnGeneration { get; }
        IUpdate<IOnMoveUpdateParameters> OnMove { get; }
        IUpdate<IOnIndividualUpdateParameters> OnIndividual { get; }
        
    }
}
