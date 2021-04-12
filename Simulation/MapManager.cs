﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
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
        private IMapCell[,] _map;
        private int _mapSize;
        private int _maxMovesWithoutFood;
        public MapManager(Action<int, int, MapCellStatus> callback, int numberOfPairs, IMapCell[,] map, int mapSize, int maxMovesWithoutFood)
        {
            _map = map;
            _mapSize = mapSize;
            _maxMovesWithoutFood = maxMovesWithoutFood;

            _agents = new Bot[numberOfPairs * 2];
            for (int i = 0; i < _agents.Length; i++)
            {
                _agents[i] = new Bot(map, mapSize, maxMovesWithoutFood,
                    new NetworkInfo(
                                     new LayerInfo(new Identity(), 2 * 4 + 8 * 3 + 6),
                                     new LayerInfo(new ReLu(), 20),
                                     new LayerInfo(new ReLu(), 12),
                                     new LayerInfo(new Sigmoid(), 4))
                    );
            }
            _callback = callback;
        }

        public void Run()
        {
            FitnessParameters fp = new(-500, -100, .5, .1, 1, .75, 500);

            int generation = 0;

            do
            {


                // run simulation
                SimulationResult[] res = new SimulationResult[_agents.Length];

                List<FitnessResults> results = new(res.Length);

                for (int i = 0; i < res.Length; i++)
                {
                    res[i] = _agents[i].Run(_callback);

                    results.Add(new FitnessResults(i, res[i].CalculateFitness(fp), _agents[i].ID));
                }
                //sort based on results
                results.Sort();

                FitnessResults rr = results[0];

                Debug.WriteLine(generation++ + " : " + rr.AgentID + " : " + rr.Result);
                //generate children
                _agents = PropagateNewGeneration(results, _agents);
                //ShuffleAgents();


            } while (true);
        }

        private void ShuffleAgents()
        {
            Random rand = new();

            for (int i = 0; i < _agents.Length; i++)
            {
                int r = rand.Next(0, _agents.Length);

                Bot temp = _agents[i];
                _agents[i] = _agents[r];
                _agents[r] = temp;
            }
        }

        private Bot[] PropagateNewGeneration(IReadOnlyList<FitnessResults> fitnessResults, Bot[] agents)
        {
            Bot[] res = new Bot[agents.Length / 2];

            int len = agents.Length / 2;

            for (int i = 0; i < len; i++)
            {
                res[i] = agents[fitnessResults[i].AgentIndex];
            }

            IMutator mutator = new BitMutator(1, .005);

            List<Bot> list = new List<Bot>(res);
            Random rand = new Random();

            List<Bot> ret = new List<Bot>();

            while(list.Count > 0)
            {
                int f = rand.Next(list.Count);
                Bot father = list[f];
                if (father == null)
                {

                }
                list.RemoveAt(f);

                int m = rand.Next(list.Count);
                Bot mother = list[m];

                if (mother == null)
                {

                }

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
