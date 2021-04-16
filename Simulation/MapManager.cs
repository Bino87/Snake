using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Network;
using Network.Mutators;
using Simulation.Core;
using Simulation.Enums;
using Simulation.SimResults;

namespace Simulation
{
    public class MapManager
    {
        private readonly Action<List<(int X, int Y, MapCellType Status)>, double[][], VisionData[]> _updateCallback;
        private Bot[] _agents;
        private readonly int _mapSize;
        private readonly int _maxMovesWithoutFood;
        private double _mutationRate;
        private double _mutationChance;

        public bool RunParallel { get; set; }

        public MapManager(Action<List<(int X, int Y, MapCellType Status)>, double[][], VisionData[]> updateCallback, int numberOfPairs, int mapSize, int maxMovesWithoutFood, NetworkInfo networkInfo, double mutationRate, double mutationChance)
        {
            _mapSize = mapSize;
            _maxMovesWithoutFood = maxMovesWithoutFood;
            _mutationChance = mutationChance;
            _mutationRate = mutationRate;

            _agents = new Bot[numberOfPairs * 2];
            for (int i = 0; i < _agents.Length; i++)
            {
                _agents[i] = new Bot(mapSize, maxMovesWithoutFood, networkInfo, 1);
            }

            _updateCallback = updateCallback;
        }

        public void Run(Action<double[][], int, int> onSimulationStart, CancellationToken token)
        {
            int generation = 0;

            do
            {
                List<FitnessResults> results = new(GetFitnessResults(onSimulationStart, generation, RunParallel));

                //sort based on results
                results.Sort();

                double avg = CalculateAverage(results);

                Debug.WriteLine(generation++ + " : " + avg.ToString("F4") + " : " + results.Max(x => x.Result.Points).ToString("F4"));

                _agents = PropagateNewGeneration(results, _agents);
                //ShuffleAgents();

            } while (true);
        }

        private FitnessResults[] GetFitnessResults(Action<double[][], int, int> onSimulationStart, int generation, bool parallel)
        {
            FitnessResults[] results = new FitnessResults[_agents.Length];
            void Run(int i)
            {
                if (!parallel)
                    onSimulationStart?.Invoke(_agents[i].GetNeuralNetwork().Weights, generation, i + 1);
                SimulationResult res = _agents[i].Run(parallel ? null : _updateCallback);

                results[i] = new FitnessResults(i, res, _agents[i].ID);
            }

            void RunItPerallel(int i)
            {
                Parallel.For(i, _agents.Length, Run);
            }

            int i = 0;
            if (parallel)
            {
                RunItPerallel(i);
            }
            else
            {
                for (; i < _agents.Length; i++)
                {
                    Run(i);
                }
            }


            return results;
        }

        private static double CalculateAverage(List<FitnessResults> results)
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
            IMutator mutator = new StringMutator(_mutationChance, _mutationRate);

            for (int i = 0; i < len; i += 2)
            {
                res[i] = agents[fitnessResults[i].AgentIndex];
                res[i + 1] = agents[fitnessResults[i + 1].AgentIndex];

                NeuralNetwork father = res[i].GetNeuralNetwork();
                NeuralNetwork mother = res[i + 1].GetNeuralNetwork();

                int generation = Math.Max(res[0].Generation, res[1].Generation);

                (NetworkInfo first, NetworkInfo second) = mutator.GetOffsprings(father, mother);

                res[i + len] = new Bot(_mapSize, _maxMovesWithoutFood, first, generation + 1);
                res[i + len + 1] = new Bot(_mapSize, _maxMovesWithoutFood, second, generation + 1);
            }

            return res;
        }
    }
}
