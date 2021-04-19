namespace Simulation.Updater
{
    public interface ISimulationUpdate
    {
        void OnMove();
        void OnIndividual();
        void OnGeneration();
    }
}
