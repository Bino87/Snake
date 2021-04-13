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
                    onUpdateWeights?.Invoke(_agents[i].GetNeuralNetwork().ToNetworkInfo().Weights);
                    res[i] = _agents[i].Run(_callback, netCallback);

                    results.Add(new FitnessResults(i, res[i].CalculateFitness(fp), _agents[i].ID));
                }
                //sort based on results
                results.Sort();

                double total = results.Sum(t => t.Result) / results.Count;


                Debug.WriteLine(generation++ + " : " + total );
                //generate children
                _agents = PropagateNewGeneration(results, _agents);
                //ShuffleAgents();


            } while (true);

        }

        private Bot[] PropagateNewGeneration(IReadOnlyList<FitnessResults> fitnessResults, IReadOnlyList<Bot> agents)

        {
            Bot[] res = new Bot[agents.Count / 2];

            int len = agents.Count / 2;

            for (int i = 0; i < len; i++)
            {
                res[i] = agents[fitnessResults[i].AgentIndex];
            }

            IMutator mutator = new BitMutator(1, .0005/*,1,3*/);

            List<Bot> list = new(res);
            Random rand = new();

            List<Bot> ret = new();

            while(list.Count > 0)
            {
                int f = rand.Next(list.Count);
                Bot father = list[f];
                list.RemoveAt(f);

                int m = rand.Next(list.Count);
                Bot mother = list[m];
                list.RemoveAt(m);

                (NetworkInfo first, NetworkInfo second) = mutator.GetOffsprings(father.GetNeuralNetwork(), mother.GetNeuralNetwork());

                ret.Add(father);
                ret.Add(mother);
                ret.Add(new Bot(_map, _mapSize, _maxMovesWithoutFood, first));
                ret.Add(new Bot(_map, _mapSize, _maxMovesWithoutFood, second));

            }

            return ret.ToArray();
        }
    }
}
