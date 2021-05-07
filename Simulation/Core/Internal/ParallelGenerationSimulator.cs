using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Simulation.Interfaces;
using Simulation.SimResults;

namespace Simulation.Core.Internal
{
    internal class ParallelGenerationSimulator : GenerationSimulator
    {
        public ParallelGenerationSimulator(Bot[] agents, ISimulationUpdateManager updateManager,
            CancellationToken cancellationToken) : base(agents, updateManager, cancellationToken)
        {

        }

        internal override IEnumerable<FitnessResults> SimulateGeneration()
        {
            Parallel.For(0, _agents.Length, (i) => { RunAgentSimulation(i); });

            return _results;
        }
    }
}