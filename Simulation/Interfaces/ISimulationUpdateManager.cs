namespace Simulation.Interfaces
{
    public interface ISimulationUpdateManager
    {
        void OnIndividual(double[][] weights, int generation, int i);
        void OnGeneration();

        IUpdate<IOnMoveUpdateParameters> OnMove { get; }
        

        //switch them to properties
            // each updater should have it's own data 
            // and update method.
    }

    public interface IUpdate<T>
    {
        bool ShouldUpdate { get; }
        T Data { get; }
        void Update();
    }

}
