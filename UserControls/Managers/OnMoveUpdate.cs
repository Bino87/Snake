using System.Collections.Generic;
using System.Windows;
using Simulation.Core;
using Simulation.Enums;
using Simulation.Interfaces;
using UserControls.Models;

namespace UserControls.Managers
{
    public class OnMoveUpdate : OnUpdateAbstract<IOnMoveUpdateParameters>
    {
        private readonly SnakeMapViewModel _snakeMapViewModel;
        private readonly NeuralNetDisplayViewModel _neuralNetDisplayViewModel;
        public override IOnMoveUpdateParameters Data { get; }
        public override void Update()
        {
            if(!ShouldUpdate)
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

    public class OnOnMoveUpdateParameters : IOnMoveUpdateParameters
    {
        public OnOnMoveUpdateParameters(SimulationGuiViewModel simulationGuiViewModel)
        {
            VisionData = new UpdateList<VisionData>(simulationGuiViewModel);
            CalculationResults = new UpdateList<double[]>(simulationGuiViewModel);
            CellUpdateData = new UpdateList<CellUpdateData>(simulationGuiViewModel);
            Points = 0;
            Moves = 0;
        }

        public bool ShouldUpdate { get; set; }
        public IList<VisionData> VisionData { get; }
        public IList<double[]> CalculationResults { get; }
        public IList<CellUpdateData> CellUpdateData { get; }
        public int Points { get; set; }
        public int Moves { get; set; }

        public void Clear()
        {
            VisionData.Clear();
            CalculationResults.Clear();
            CellUpdateData.Clear();
        }
    }
}