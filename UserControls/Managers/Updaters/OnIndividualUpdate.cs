using System.Windows;
using Simulation.Interfaces;
using UserControls.Models;

namespace UserControls.Managers.Updaters
{
    public class OnIndividualUpdate : OnUpdateAbstract<IOnIndividualUpdateParameters>
    {
        private readonly NeuralNetDisplayViewModel _neuralNetDisplayViewModel;

        public OnIndividualUpdate(ISimulationUpdateManager simulationUpdateManager, NeuralNetDisplayViewModel neuralNetDisplayViewModel, ISimulationStateParameters simulationGuiViewModel) : base(simulationUpdateManager, simulationGuiViewModel)
        {
            _neuralNetDisplayViewModel = neuralNetDisplayViewModel;
            Data = new OnIndividualUpdateParameters(simulationUpdateManager);
        }

        public override IOnIndividualUpdateParameters Data { get; }
        public override void Update()
        {

            if (!ShouldUpdate)
                return;

            Application.Current?.Dispatcher.Invoke(() =>
                {
                    _neuralNetDisplayViewModel.UpdateWeights(Data.Weights);
                    _simulationGuiViewModel.CurrentIndividual = Data.IndividualIndex;
                }
            );

            Data.Clear();
            DelaySim();
        }
    }
}