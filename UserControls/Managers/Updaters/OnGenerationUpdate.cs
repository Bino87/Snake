using System.Windows;
using Simulation.Interfaces;

namespace UserControls.Managers.Updaters
{
    public class OnGenerationUpdate : OnUpdateAbstract<IOnGenerationUpdateParameters>
    {

        public override bool ShouldUpdate => true;
        public override IOnGenerationUpdateParameters Data { get; }


        public OnGenerationUpdate(ISimulationStateParameters simulationGuiViewModel) : base(simulationGuiViewModel)
        {
            Data = new OnGenerationUpdateParameters();
        }

        public override void Update()
        {
            Application.Current?.Dispatcher.Invoke(() =>
                {
                    _simulationGuiViewModel.Generation = Data.Generation;
                }
            );
        }
    }
}