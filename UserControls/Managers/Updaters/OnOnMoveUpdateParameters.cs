using System.Collections.Generic;
using Simulation.Core;
using Simulation.Interfaces;

namespace UserControls.Managers.Updaters
{
    public class OnOnMoveUpdateParameters : IOnMoveUpdateParameters
    {
        public OnOnMoveUpdateParameters(ISimulationStateParameters simulationGuiViewModel)
        {
            VisionData = new UpdateList<VisionData>(simulationGuiViewModel);
            CalculationResults = new UpdateList<double[]>(simulationGuiViewModel);
            CellUpdateData = new UpdateList<CellUpdateData>(simulationGuiViewModel);
            Points = 0;
            Moves = 0;
        }

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