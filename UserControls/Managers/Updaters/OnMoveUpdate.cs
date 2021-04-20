using System.Windows;
using Simulation.Interfaces;
using UserControls.Models;

namespace UserControls.Managers.Updaters
{
    public class OnMoveUpdate : OnUpdateAbstract<IOnMoveUpdateParameters>
    {
        private readonly SnakeMapViewModel _snakeMapViewModel;
        private readonly NeuralNetDisplayViewModel _neuralNetDisplayViewModel;
        public override IOnMoveUpdateParameters Data { get; }
        public override void Update()
        {
            if (!ShouldUpdate)
                return;

            Application.Current?.Dispatcher.Invoke(() =>
            {
                _snakeMapViewModel.SetCells(Data.CellUpdateData, Data.VisionData, _simulationGuiViewModel.MapSize);
                _neuralNetDisplayViewModel.OnResultsCalculated(Data.CalculationResults);
                _simulationGuiViewModel.Moves = Data.Moves;
                _simulationGuiViewModel.Points = Data.Points;
            });

            Data.Clear();
            DelaySim();
        }

        public OnMoveUpdate(SimulationGuiViewModel simulationGuiViewModel, SnakeMapViewModel snakeMapViewModel, NeuralNetDisplayViewModel neuralNetDisplayViewModel) : base(simulationGuiViewModel)
        {
            _snakeMapViewModel = snakeMapViewModel;
            _neuralNetDisplayViewModel = neuralNetDisplayViewModel;
            Data = new OnOnMoveUpdateParameters(simulationGuiViewModel);
        }
    }
}