using Simulation.Interfaces;
using UserControls.Managers.Updaters;
using UserControls.Models;

namespace UserControls.Managers
{
    public class SimulationUpdateManager : ISimulationUpdateManager
    {
        private readonly NeuralNetDisplayViewModel _neuralNetDisplayViewModel;
        private readonly SimulationGuiViewModel _simulationGuiViewModel;


        public IUpdate<IOnGenerationUpdateParameters> OnGeneration { get; }
        public IUpdate<IOnMoveUpdateParameters> OnMove { get; }
        public IUpdate<IOnIndividualUpdateParameters> OnIndividual { get; }

        public bool UpdateOnMove => !_simulationGuiViewModel.RunInBackground;


        public SimulationUpdateManager(NeuralNetDisplayViewModel neuralNetDisplayViewModel, SimulationGuiViewModel simulationGuiViewModel, SnakeMapViewModel snakeMapViewModel)
        {
            _neuralNetDisplayViewModel = neuralNetDisplayViewModel;
            _simulationGuiViewModel = simulationGuiViewModel;
            OnMove = new OnMoveUpdate(simulationGuiViewModel, snakeMapViewModel, neuralNetDisplayViewModel);
            OnIndividual = new OnIndividualUpdate(simulationGuiViewModel, neuralNetDisplayViewModel);
            OnGeneration = new OnGenerationUpdate(simulationGuiViewModel);
        }

    }
}
