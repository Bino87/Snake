using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
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

        public void Run(Action<double[][], int,int> onSimulationStart, CancellationToken token)
        {
            int generation = 0;

            do
            {
                List<FitnessResults> results = new(_agents.Length);

                for (int i = 0; i < _agents.Length; i++)
                {
                    if (token.IsCancellationRequested)
                        return;

                    onSimulationStart?.Invoke(_agents[i].GetNeuralNetwork().Weights, generation, i+1);
                    SimulationResult res = _agents[i].Run(_updateCallback);

                    results.Add(new FitnessResults(i, res, _agents[i].ID));
                }

                //sort based on results
                results.Sort();

                double avg = CalculateAverage(results);

                Debug.WriteLine(generation++ + " : " + avg.ToString("F4") + " : " + results.Max(x => x.Result.Points).ToString("F4"));

                _agents = PropagateNewGeneration(results, _agents);
                ShuffleAgents();

            } while (true);
        }

        void ShuffleAgents()
        {
            Random random = new Random();
            for (int i = 0; i < _agents.Length; i++)
            {
                int index = random.Next(_agents.Length);
                Bot temp = _agents[i];
                _agents[i] = _agents[index];
                _agents[index] = temp;
            }
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

            for (int i = 0; i < len ; i+= 2)
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
