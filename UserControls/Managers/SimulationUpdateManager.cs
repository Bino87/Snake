using Simulation.Interfaces;
using UserControls.Managers.Updaters;
using UserControls.Models;
using UserControls.Models.NeuralNetDisplay;
using UserControls.Models.SimulationRunner;

namespace UserControls.Managers
{
    public class SimulationUpdateManager : ISimulationUpdateManager
    {
        public bool ShouldUpdate { get; set; }
        public IUpdate<IOnGenerationUpdateParameters> OnGeneration { get; }
        public IUpdate<IOnMoveUpdateParameters> OnMove { get; }
        public IUpdate<IOnIndividualUpdateParameters> OnIndividual { get; }
        public void Clear()
        {
            OnIndividual.Clear();
            OnMove.Clear();
            OnGeneration.Clear();
        }


        public SimulationUpdateManager(NeuralNetDisplayViewModel neuralNetDisplayViewModel, ISimulationStateParameters simulationGuiViewModel, SnakeMapViewModel snakeMapViewModel, IProgressGraphValueRegister progressGraphViewModel)
        {
            OnMove = new OnMoveUpdate(this, snakeMapViewModel, neuralNetDisplayViewModel, simulationGuiViewModel);
            OnIndividual = new OnIndividualUpdate(this, neuralNetDisplayViewModel, simulationGuiViewModel);
            OnGeneration = new OnGenerationUpdate(this, progressGraphViewModel, simulationGuiViewModel);
        }
    }
}
