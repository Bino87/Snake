using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Network;
using Network.ActivationFunctions;
using Network.Mutators;
using Simulation.Enums;
using Simulation.Interfaces;
using Simulation.SimResults;

namespace Simulation
{
    public class MapManager
    {
        private readonly Action<int, int, MapCellStatus> _callback;
        private Bot[] _agents;
        private readonly IMapCell[,] _map;
        private readonly int _mapSize;
        private readonly int _maxMovesWithoutFood;
        private Action<double[][]> netCallback;
        public Bot[] Agents => _agents;

        public MapManager(Action<int, int, MapCellStatus> callback, Action<double[][]> netCallback, int numberOfPairs, IMapCell[,] map, int mapSize, int maxMovesWithoutFood, NetworkInfo networkInfo)
        {
            _map = map;
            _mapSize = mapSize;
            _maxMovesWithoutFood = maxMovesWithoutFood;

            _agents = new Bot[numberOfPairs * 2];
            for (int i = 0; i < _agents.Length; i++)
            {
                _agents[i] = new Bot(map, mapSize, maxMovesWithoutFood, networkInfo);
            }
            _callback = callback;
            this.netCallback = netCallback;
        }

        public void Run(Action<double[][]> onUpdateWeights)
        {
            FitnessParameters fp = new(-500, -100, .5, .1, 1, .75, 500);

            int generation = 0;

            do
            {
                SimulationResult[] res = new SimulationResult[_agents.Length];

                List<FitnessResults> results = new(res.Length);

                for (int i = 0; i < res.Length; i++)
                {
                    onUpdateWeights?.Invoke(_agents[i].GetNeuralNetwork().Weights);
                    res[i] = _agents[i].Run(_callback, netCallback);

                    results.Add(new FitnessResults(i, res[i].CalculateFitness(fp), _agents[i].ID));
                }
                //sort based on results
                results.Sort();

                double total = 0;

                for(int i = 0; i < results.Count /2; i++)
                {
                    total += results[i].Result;
                }

                total /= results.Count / 2d;

                Debug.WriteLine(generation++ + " : " + total.ToString("F4") + " : " + results.Max(x => x.Result).ToString("F4"));
                //generate children
                _agents = PropagateNewGeneration(results, _agents);
                //ShuffleAgents();


            } while (true);

        }

        private Bot[] PropagateNewGeneration(IReadOnlyList<FitnessResults> fitnessResults, IReadOnlyList<Bot> agents)

        {
            Bot[] res = new Bot[agents.Count];

            int len = agents.Count / 2;

            for (int i = 0; i < len; i++)
            {
                res[i] = agents[fitnessResults[i].AgentIndex];
            }

            IMutator mutator = new BitMutator(1, .02);
            Bot father = res[0];

            for(int i = len - 1; i >= 0; i--)
            {
                if (i == 0)
                    father = res[len - 1];

                Bot mother = res[i];

                (NetworkInfo first, NetworkInfo second) = mutator.GetOffsprings(father.GetNeuralNetwork(), mother.GetNeuralNetwork());

                res[i + len ] = new Bot(_map, _mapSize, _maxMovesWithoutFood, first);
                //res[i + len + 1] = new Bot(_map, _mapSize, _maxMovesWithoutFood, second);
            }


            return res;
        }
    }
}
