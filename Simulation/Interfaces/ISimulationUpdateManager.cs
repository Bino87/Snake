namespace Simulation.Interfaces
{
    public interface ISimulationUpdateManager
    {
        public bool ShouldUpdate { get; set; }
        IUpdate<IOnGenerationUpdateParameters> OnGeneration { get; }
        IUpdate<IOnMoveUpdateParameters> OnMove { get; }
        IUpdate<IOnIndividualUpdateParameters> OnIndividual { get; }

        void Clear();

    }
}
