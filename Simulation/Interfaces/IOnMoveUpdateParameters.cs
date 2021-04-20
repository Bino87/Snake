using System.Collections.Generic;
using Simulation.Core;

namespace Simulation.Interfaces
{
    public interface IOnMoveUpdateParameters : IUpdaterParameters
    {
        IList<VisionData> VisionData { get; }
        IList<double[]> CalculationResults { get; }
        IList<CellUpdateData> CellUpdateData { get; }
        int Points { get; set; }
        int Moves { get; set; }

        void Clear();
    }

    public interface IOnIndividualUpdateParameters : IUpdaterParameters
    {
        double[][] Weights { get; }
        int Generation { get; }
        int IndividualIndex { get; }
    }

    public interface IUpdaterParameters
    {
        bool ShouldUpdate { get; set; }
    }
}