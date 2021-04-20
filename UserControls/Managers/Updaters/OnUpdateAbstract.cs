using System.Threading;
using Simulation.Interfaces;
using UserControls.Models;

namespace UserControls.Managers.Updaters
{
    public abstract class OnUpdateAbstract<T> : IUpdate<T>
    {
        protected SimulationGuiViewModel _simulationGuiViewModel;

        protected OnUpdateAbstract(SimulationGuiViewModel simulationGuiViewModel)
        {
            _simulationGuiViewModel = simulationGuiViewModel;
        }

        public virtual bool ShouldUpdate => !_simulationGuiViewModel.RunInBackground;

        public abstract T Data { get; }
        public abstract void Update();

        protected void DelaySim()
        {
            Thread.Sleep(_simulationGuiViewModel.UpdateDelay);
        }
    }
}