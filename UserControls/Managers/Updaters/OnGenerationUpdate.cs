using System.Windows;
using Simulation.Interfaces;
using UserControls.Models;

namespace UserControls.Managers.Updaters
{
    public class OnGenerationUpdate : OnUpdateAbstract<IOnGenerationUpdateParameters>
    {
        private IProgressGraphValueRegister _progressGraphValueRegister;
        public override bool ShouldUpdate => true;
        public override IOnGenerationUpdateParameters Data { get; }


        public OnGenerationUpdate(ISimulationStateParameters simulationGuiViewModel, IProgressGraphValueRegister progressGraphValueRegister) : base(simulationGuiViewModel)
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
    }
}