using System.Collections.Generic;
using System.Threading;
using Simulation.Extensions;
using Simulation.Interfaces;
using Simulation.SimResults;

namespace Simulation.Core.Internal
{
    internal abstract class GenerationSimulator
    {
        protected Bot[] _agents;
        protected readonly ISimulationUpdateManager _updateManager;
        protected FitnessResults[] _results;
        private CancellationToken _cancellationToken;

        protected GenerationSimulator(Bot[] agents, ISimulationUpdateManager updateManager,
            CancellationToken cancellationToken)
        {
            _cancellationToken = cancellationToken;
            _agents = agents;
            _updateManager = updateManager;
            _results = new FitnessResults[_agents.Length];
        }

        internal abstract IEnumerable<FitnessResults> SimulateGeneration();

        protected bool RunAgentSimulation(int index)
        {
            if(_cancellationToken.IsCancellationRequested)
                return true;

            _updateManager.UpdateIndividual(_agents[index].GetNeuralNetwork().Weights, index);

            SimulationResult res = _agents[index].Run(_updateManager.OnMove);

            _results[index] = new FitnessResults(index, res, _agents[index].Id);
            return false;
        }
    }
}