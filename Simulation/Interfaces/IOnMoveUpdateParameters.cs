using System.Collections.Generic;
using Simulation.Core;

namespace Simulation.Interfaces
{
    public interface IOnMoveUpdateParameters : IUpdateParameters
    {
        IList<VisionData> VisionData { get; }
        IList<double[]> CalculationResults { get; }
        IList<CellUpdateData> CellUpdateData { get; }
        int Points { get; set; }
        int Moves { get; set; }
        void Clear();

    }
}