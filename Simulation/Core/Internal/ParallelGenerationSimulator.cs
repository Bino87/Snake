using System.Collections.Generic;
using System.Threading.Tasks;
using Simulation.Interfaces;
using Simulation.SimResults;

namespace Simulation.Core.Internal
{
    internal class ParallelGenerationSimulator : GenerationSimulator
    {
        public ParallelGenerationSimulator(Bot[] agents, ISimulationUpdateManager updateManager) : base(agents, updateManager)
        {

        }

        internal override IEnumerable<FitnessResults> SimulateGeneration()
        {
            Parallel.For(0, _agents.Length, RunAgentSimulation);

            return _results;
        }
    }
}