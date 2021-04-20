using Simulation.Interfaces;
using UserControls.Managers.Updaters;
using UserControls.Models;

namespace UserControls.Managers
{
    public class SimulationUpdateManager : ISimulationUpdateManager
    {
        public IUpdate<IOnGenerationUpdateParameters> OnGeneration { get; }
        public IUpdate<IOnMoveUpdateParameters> OnMove { get; }
        public IUpdate<IOnIndividualUpdateParameters> OnIndividual { get; }


        public SimulationUpdateManager(NeuralNetDisplayViewModel neuralNetDisplayViewModel, ISimulationStateParameters simulationGuiViewModel, SnakeMapViewModel snakeMapViewModel, IProgressGraphValueRegister progressGraphViewModel)
        {
            OnMove = new OnMoveUpdate(simulationGuiViewModel, snakeMapViewModel, neuralNetDisplayViewModel);
            OnIndividual = new OnIndividualUpdate(simulationGuiViewModel, neuralNetDisplayViewModel);
            OnGeneration = new OnGenerationUpdate(simulationGuiViewModel, progressGraphViewModel);
        }

    }
}
