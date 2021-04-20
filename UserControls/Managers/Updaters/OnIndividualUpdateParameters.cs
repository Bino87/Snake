using System.Collections.Generic;
using Simulation.Interfaces;

namespace UserControls.Managers.Updaters
{
    public class OnIndividualUpdateParameters : IOnIndividualUpdateParameters
    {
        public OnIndividualUpdateParameters(ISimulationStateParameters simulationGuiViewModel)
        {
            Weights = new UpdateList<double[]>(simulationGuiViewModel);
        }

        public void Clear()
        {
            Weights.Clear();
        }

        public IList<double[]> Weights { get; }
        public int IndividualIndex { get; set; }
    }
}