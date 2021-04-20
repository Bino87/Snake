using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Network;
using Network.Mutators;
using Simulation.Interfaces;
using Simulation.SimResults;

namespace Simulation
{
    public class MapManager
    {
        private Bot[] _agents;
        private readonly ISimulationStateParameters _simStateParameters;
        private readonly ISimulationUpdateManager _updateManager;
        private readonly NetworkInfo _networkInfo;

        public MapManager( ISimulationStateParameters simStateParameters, NetworkInfo networkInfo, ISimulationUpdateManager updateManager)
        {
            _simStateParameters = simStateParameters;
            _networkInfo = networkInfo;
            _updateManager = updateManager;
        }

        private void InitializeAgents()
        {
            _agents = new Bot[_simStateParameters.NumberOfPairs * 2];
            for (int i = 0; i < _agents.Length; i++)
            {
                _agents[i] = new Bot(_simStateParameters.MapSize, _simStateParameters.MaxMoves, _networkInfo, 1);
            }
        }

        public void Run()
        {
            int generation = 0;
            InitializeAgents();
            RunSimulation( generation);
        }

        private void RunSimulation(int generation)
        {
            do
            {
                List<FitnessResults> results = new(GetFitnessResults( generation, _simStateParameters.RunInBackground));

                results.Sort();

                double avg = CalculateAverage(results);

                Debug.WriteLine(generation++ + " : " + avg.ToString("F4") + " : " + results.Max(x => x.Result.Points).ToString("F4"));

                _agents = PropagateNewGeneration(results, _agents);

                //updateManager
                _updateManager.OnGeneration();

                if(false)
                    return;

            } while(true);
        }

        private IEnumerable<FitnessResults> GetFitnessResults( int generation, bool parallel)
        {
            FitnessResults[] results = new FitnessResults[_agents.Length];
            
            void RunLocal(int i)
            {
                if (!parallel)
                {
                    _updateManager.OnIndividual(_agents[i].GetNeuralNetwork().Weights, generation, i + 1);
                }
                SimulationResult res = _agents[i].Run(_updateManager.OnMove);

                results[i] = new FitnessResults(i, res, _agents[i].Id);
            }

            void RunItParallel(int i)
            {
                Parallel.For(i, _agents.Length, RunLocal);
            }


            if (parallel)
            {
                RunItParallel(0);
            }
            else
            {
                for (int i = 0; i < _agents.Length; i++)
                {
                    RunLocal(i);

                    _simStateParameters.CurrentIndividual = i;

                    if (_simStateParameters.RunInBackground)
                    {
                        parallel = _simStateParameters.RunInBackground;
                        RunItParallel(i + 1);
                        break;
                    }
                }
            }


            return results;
        }

        private static double CalculateAverage(IReadOnlyList<FitnessResults> results)
        {
            int len = (int)(results.Count * .5);
            double total = 0;

            for (int i = 0; i < len; i++)
            {
                total += results[i].Result.Points;
            }

            total /= len;
            return total;
        }

        private Bot[] PropagateNewGeneration(IReadOnlyList<FitnessResults> fitnessResults, IReadOnlyList<Bot> agents)

        {
            Bot[] res = new Bot[agents.Count];

            int len = res.Length / 2;
            IMutator mutator = new StringMutator(_simStateParameters.MutationChance, _simStateParameters.MutationRate);

            for (int i = 0; i < len; i += 2)
            {
                res[i] = agents[fitnessResults[i].AgentIndex];
                res[i + 1] = agents[fitnessResults[i + 1].AgentIndex];

                NeuralNetwork father = res[i].GetNeuralNetwork();
                NeuralNetwork mother = res[i + 1].GetNeuralNetwork();

                int generation = Math.Max(res[0].Generation, res[1].Generation);

                (NetworkInfo first, NetworkInfo second) = mutator.GetOffsprings(father, mother);

                res[i + len] = new Bot(_simStateParameters.MapSize, _simStateParameters.MaxMoves, first, generation + 1);
                res[i + len + 1] = new Bot(_simStateParameters.MapSize, _simStateParameters.MaxMoves, second, generation + 1);
            }

            return res;
        }
    }
}
