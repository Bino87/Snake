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


        public void InitializeAgents(NetworkTemplate networkTemplate)
        {
            _agents = new Bot[_simStateParameters.NumberOfPairs * 2];
            for (int i = 0; i < _agents.Length; i++)
            {
                _agents[i] = new Bot(networkTemplate.ToNetworkData(), _simStateParameters.MapSize, _simStateParameters.MaxMoves, 1,  _simStateParameters.NumberOfIterations);
            }
        }

        public void PropagateNewGeneration(IReadOnlyList<FitnessResults> fitnessResults, int generation)
        {
            Bot[] res = new Bot[_agents.Length];

            int len = res.Length / 2;
            IMutator mutator = _simStateParameters.MutationTechnique.GetMutator(_simStateParameters.MutationChance, _simStateParameters.MutationRate);


            Parallel.For(0, len / 2, (i) =>
            {
                i *= 2;
                int x = i + 1;

                res[i] = _agents[fitnessResults[i].AgentIndex];
                res[x] = _agents[fitnessResults[x].AgentIndex];

                BasicNeuralNetwork parent1 = res[i].GetNeuralNetwork();
                BasicNeuralNetwork parent2 = res[x].GetNeuralNetwork();

                (NetworkData first, NetworkData second) = mutator.Get2Offsprings(parent1, parent2);

                res[i + len] = new Bot(first, _simStateParameters.MapSize, _simStateParameters.MaxMoves, generation + 1,  _simStateParameters.NumberOfIterations);
                res[x + len] = new Bot(second, _simStateParameters.MapSize, _simStateParameters.MaxMoves, generation + 1, _simStateParameters.NumberOfIterations);
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