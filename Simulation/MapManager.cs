using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Network;
using Network.Mutators;
using Simulation.Core;
using Simulation.Enums;
using Simulation.Interfaces;
using Simulation.SimResults;

namespace Simulation
{
    public class MapManager
    {
        private readonly Action<List<(int X, int Y, MapCellType Status)>, double[][], VisionData[]> _updateCallback;
        private Bot[] _agents;
        private ISimulationStateParameters _simStateParameters;
        private NetworkInfo _networkInfo;

        public MapManager(Action<List<(int X, int Y, MapCellType Status)>, double[][], VisionData[]> updateCallback, ISimulationStateParameters simStateParameters, NetworkInfo networkInfo)
        {
            _simStateParameters = simStateParameters;
            _networkInfo = networkInfo;
            _updateCallback = updateCallback;
        }

        private void InitializeAgents()
        {
            _agents = new Bot[_simStateParameters.NumberOfPairs * 2];
            for (int i = 0; i < _agents.Length; i++)
            {
                _agents[i] = new Bot(_simStateParameters.MapSize, _simStateParameters.MaxMoves, _networkInfo, 1);
            }
        }

        public void Run(Action<double[][], int, int> onSimulationStart)
        {
            int generation = 0;
            InitializeAgents();
            do
            {
                List<FitnessResults> results = new(GetFitnessResults(onSimulationStart, generation, _simStateParameters.RunInBackground));
                
                results.Sort();

                double avg = CalculateAverage(results);

                Debug.WriteLine(generation++ + " : " + avg.ToString("F4") + " : " + results.Max(x => x.Result.Points).ToString("F4"));

                _agents = PropagateNewGeneration(results, _agents);
                
                //update

            } while (true);
        }

        private IEnumerable<FitnessResults> GetFitnessResults(Action<double[][], int, int> onSimulationStart, int generation, bool parallel)
        {
            FitnessResults[] results = new FitnessResults[_agents.Length];
            
            void RunLocal(int i)
            {
                if (!parallel)
                    onSimulationStart?.Invoke(_agents[i].GetNeuralNetwork().Weights, generation, i + 1);
                SimulationResult res = _agents[i].Run(parallel ? null : _updateCallback);

                results[i] = new FitnessResults(i, res, _agents[i].ID);
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
