﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Network;
using Network.Mutators;
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

        public void PropagateNewGeneration(IReadOnlyList<FitnessResults> fitnessResults)
        {
            Bot[] res = new Bot[_agents.Length];

            int len = res.Length / 2;
            IMutator mutator = _simStateParameters.MutationTechnique.GetMutator(_simStateParameters.MutationChance, _simStateParameters.MutationRate);

            for (int i = 0; i < len; i += 2)
            {
                res[i] = _agents[fitnessResults[i].AgentIndex];
                res[i + 1] = _agents[fitnessResults[i + 1].AgentIndex];

                NeuralNetwork father = res[i].GetNeuralNetwork();
                NeuralNetwork mother = res[i + 1].GetNeuralNetwork();

                int generation = Math.Max(res[0].Generation, res[1].Generation);

                (NetworkInfo first, NetworkInfo second) = mutator.GetOffsprings(father, mother);

                res[i + len] = new Bot(_simStateParameters.MapSize, _simStateParameters.MaxMoves, first, generation + 1);
                res[i + len + 1] = new Bot(_simStateParameters.MapSize, _simStateParameters.MaxMoves, second, generation + 1);
            }

            _agents = res;
        }



        public IEnumerable<FitnessResults> GetFitnessResults()
        {
            FitnessResults[] results = new FitnessResults[_agents.Length];

            void RunAgentSimulation(int i)
            {
                if (_updateManager.OnIndividual.ShouldUpdate)
                {
                    double[][] weights = _agents[i].GetNeuralNetwork().Weights;

                    foreach (double[] t in weights)
                    {
                        _updateManager.OnIndividual.Data.Weights.Add(t);
                    }

                    _updateManager.OnIndividual.Data.IndividualIndex = i;
                    _updateManager.OnIndividual.Update();
                }

                SimulationResult res = _agents[i].Run(_updateManager.OnMove);

                results[i] = new FitnessResults(i, res, _agents[i].Id);
            }

            void RunItParallel(int i)
            {
                Parallel.For(i, _agents.Length, RunAgentSimulation);
            }


            if (_simStateParameters.RunInBackground)
            {
                RunItParallel(0);
            }
            else
            {
                for (int i = 0; i < _agents.Length; i++)
                {
                    RunAgentSimulation(i);
                }
            }


            return results;
        }
    }
}