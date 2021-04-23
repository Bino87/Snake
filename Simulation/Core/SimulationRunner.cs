using System.Collections.Generic;
using System.Threading.Tasks;
using Network;
using Network.Mutators;
using Simulation.Core.Internal;
using Simulation.Extensions;
using Simulation.Interfaces;
using Simulation.SimResults;

namespace Simulation.Core
{
    public class SimulationRunner
    {
        private Bot[] _agents;
        private readonly ISimulationStateParameters _simStateParameters;
        private readonly ISimulationUpdateManager _updateManager;

        public SimulationRunner(ISimulationStateParameters simStateParameters, ISimulationUpdateManager simulationUpdateManager)
        {
            _simStateParameters = simStateParameters;
            _updateManager = simulationUpdateManager;
        }


        public void InitializeAgents(NetworkInfo networkInfo)
        {
            _agents = new Bot[_simStateParameters.NumberOfPairs * 2];
            for (int i = 0; i < _agents.Length; i++)
            {
                _agents[i] = new Bot(_simStateParameters.MapSize, _simStateParameters.MaxMoves, networkInfo, 1);
            }
        }

        public void PropagateNewGeneration(IReadOnlyList<FitnessResults> fitnessResults, int generation)
        {
            Bot[] res = new Bot[_agents.Length];

            int len = res.Length / 2;
            IMutator mutator = _simStateParameters.MutationTechnique.GetMutator(_simStateParameters.MutationChance, _simStateParameters.MutationRate);

            Parallel.For(0, len / 2, (i) =>
            {
                i = i * 2;
                int x = i + 1;

                res[i] = _agents[fitnessResults[i].AgentIndex];
                res[x] = _agents[fitnessResults[x].AgentIndex];

                NeuralNetwork parent1 = res[i].GetNeuralNetwork();
                NeuralNetwork parent2 = res[x].GetNeuralNetwork();

                (NetworkInfo first, NetworkInfo second) = mutator.Get2Offsprings(parent1, parent2);

                res[i + len] = new Bot(_simStateParameters.MapSize, _simStateParameters.MaxMoves, first, generation + 1);
                res[x + len] = new Bot(_simStateParameters.MapSize, _simStateParameters.MaxMoves, second, generation + 1);
            });

            _agents = res;
        }


        public IEnumerable<FitnessResults> GetFitnessResults()
        {
            GenerationSimulator gs = _simStateParameters.RunInBackground ? new ParallelGenerationSimulator(_agents, _updateManager) : new LinearGenerationSimulator(_agents, _updateManager, _simStateParameters);

            return gs.SimulateGeneration();
        }
    }
}