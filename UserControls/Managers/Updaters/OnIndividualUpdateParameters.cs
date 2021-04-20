using System.Collections.Generic;
using Simulation.Interfaces;
using UserControls.Models;

namespace UserControls.Managers.Updaters
{
    public class OnIndividualUpdateParameters : IOnIndividualUpdateParameters
    {
        public OnIndividualUpdateParameters(SimulationGuiViewModel simulationGuiViewModel)
        {
            Weights = new UpdateList<double[]>(simulationGuiViewModel);
        }

        public void Clear()
        {
            Weights.Clear();
        }

        public IList<double[]> Weights { get; }
        public int Generation { get; set; }
        public int IndividualIndex { get; set; }
    }
}