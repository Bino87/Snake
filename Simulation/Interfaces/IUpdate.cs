namespace Simulation.Interfaces
{
    public interface IUpdate<T>
    {
        bool ShouldUpdate { get; }
        T Data { get; }
        void Update();
    }
}