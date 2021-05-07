using System.Collections.Generic;
using System.Threading;
using Simulation.Interfaces;
using Simulation.SimResults;

namespace Simulation.Core.Internal
{
    internal class LinearGenerationSimulator : GenerationSimulator
    {
        private readonly ISimulationStateParameters _simStateParameters;

        public LinearGenerationSimulator(Bot[] agents, ISimulationUpdateManager updateManager,
            ISimulationStateParameters simulationStateParameters, CancellationToken cancellationToken) : base(agents, updateManager, cancellationToken)
        {
            _simStateParameters = simulationStateParameters;
        }

        internal override IEnumerable<FitnessResults> SimulateGeneration()
        {
            for (int i = 0; i < _agents.Length; i++)
            {
                if(RunAgentSimulation(i))
                    break;

                if (_simStateParameters.RunInBackground && _updateManager.ShouldUpdate)
                    _updateManager.ShouldUpdate = false;
            }

            return _results;
        }
    }
}