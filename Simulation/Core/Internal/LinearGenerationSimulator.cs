using System.Collections.Generic;
using Simulation.Interfaces;
using Simulation.SimResults;

namespace Simulation.Core.Internal
{
    internal class LinearGenerationSimulator : GenerationSimulator
    {
        private readonly ISimulationStateParameters _simStateParameters;

        public LinearGenerationSimulator(Bot[] agents, ISimulationUpdateManager updateManager,
            ISimulationStateParameters simulationStateParameters) : base(agents, updateManager)
        {
            _simStateParameters = simulationStateParameters;
        }

        internal override IEnumerable<FitnessResults> SimulateGeneration()
        {
            for (int i = 0; i < _agents.Length; i++)
            {
                RunAgentSimulation(i);

                if (_simStateParameters.RunInBackground && _updateManager.ShouldUpdate)
                    _updateManager.ShouldUpdate = false;
            }

            return _results;
        }
    }
}