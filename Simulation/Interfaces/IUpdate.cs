namespace Simulation.Interfaces
{
    public interface IUpdate<out T>
    {
        bool ShouldUpdate { get; }
        T Data { get; }
        void Update();
    }
}