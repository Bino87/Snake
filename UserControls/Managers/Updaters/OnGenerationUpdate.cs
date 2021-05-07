using System.Windows;
using Simulation.Interfaces;
using UserControls.Models;
using UserControls.Models.SimulationRunner;

namespace UserControls.Managers.Updaters
{
    public class OnGenerationUpdate : OnUpdateAbstract<IOnGenerationUpdateParameters>
    {
        private readonly IProgressGraphValueRegister _progressGraphValueRegister;
        public override bool ShouldUpdate => true;
        public override IOnGenerationUpdateParameters Data { get; }


        public OnGenerationUpdate(ISimulationUpdateManager simulationUpdateManager, IProgressGraphValueRegister progressGraphValueRegister, ISimulationStateParameters simulationGuiViewModel) : base(simulationUpdateManager,simulationGuiViewModel)
        {
            Data = new OnGenerationUpdateParameters();
            _progressGraphValueRegister = progressGraphValueRegister;
        }

        public override void Update()
        {
            Application.Current?.Dispatcher.Invoke(() =>
                {
                    _simulationGuiViewModel.Generation = Data.Generation;
                    _progressGraphValueRegister.Register(Data.AverageFitnessValue);
                }
            );
        }

        public override void Clear()
        {
            _progressGraphValueRegister.Clear();
        }
    }
}