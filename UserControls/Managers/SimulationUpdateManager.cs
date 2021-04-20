using System;
using System.Windows;
using Simulation.Interfaces;
using UserControls.Models;

namespace UserControls.Managers
{
    public class SimulationUpdateManager : ISimulationUpdateManager
    {
        private readonly NeuralNetDisplayViewModel _neuralNetDisplayViewModel;
        private readonly SimulationGuiViewModel _simulationGuiViewModel;

        public IUpdate<IOnMoveUpdateParameters> OnMove { get; }

        public bool UpdateOnMove => !_simulationGuiViewModel.RunInBackground;


        public SimulationUpdateManager(NeuralNetDisplayViewModel neuralNetDisplayViewModel, SimulationGuiViewModel simulationGuiViewModel, SnakeMapViewModel snakeMapViewModel)
        {
            _neuralNetDisplayViewModel = neuralNetDisplayViewModel;
            _simulationGuiViewModel = simulationGuiViewModel;
            OnMove = new OnMoveUpdate(simulationGuiViewModel, snakeMapViewModel, neuralNetDisplayViewModel);
        }

        public void OnIndividual(double[][] weights, int generation, int i)
        {
            if (!UpdateOnMove)
                return;

            Application.Current?.Dispatcher.Invoke(() =>
                {
                    _neuralNetDisplayViewModel.UpdateWeights(weights);
                    _simulationGuiViewModel.Generation = generation;
                }
            );

        }

        public void OnGeneration()
        {
            //throw new NotImplementedException();
        }


    }
}
