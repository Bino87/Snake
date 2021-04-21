using System.Threading;
using Simulation.Interfaces;

namespace UserControls.Managers.Updaters
{
    public abstract class OnUpdateAbstract<T> : IUpdate<T>
    {
        protected ISimulationUpdateManager _simulationUpdateManager;
        protected ISimulationStateParameters _simulationGuiViewModel;

        protected OnUpdateAbstract(ISimulationUpdateManager simulationUpdateManager, ISimulationStateParameters simulationGuiViewModel)
        {
            _simulationUpdateManager = simulationUpdateManager;
            _simulationGuiViewModel = simulationGuiViewModel;

        }

        public virtual bool ShouldUpdate => _simulationUpdateManager.ShouldUpdate;

        public abstract T Data { get; }
        public abstract void Update();

        protected void DelaySim()
        {
            Thread.Sleep(_simulationGuiViewModel.UpdateDelay);
        }
    }
}