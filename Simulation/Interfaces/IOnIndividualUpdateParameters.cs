using System.Collections.Generic;

namespace Simulation.Interfaces
{
    public interface IOnIndividualUpdateParameters : IUpdateParameters
    {
        IList<double[]> Weights { get; }
        int Generation { get; set; }
        int IndividualIndex { get; set; }

    }
}