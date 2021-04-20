using System.Collections.Generic;

namespace Simulation.Interfaces
{
    public interface IOnIndividualUpdateParameters : IUpdateParameters
    {
        IList<double[]> Weights { get; }
        int IndividualIndex { get; set; }

    }
}