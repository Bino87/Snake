﻿using System.Collections.Generic;
using System.Diagnostics;
using Network;
using Simulation.Core;
using Simulation.Extensions;
using Simulation.Interfaces;
using Simulation.SimResults;

namespace Simulation
{
    public class MapManager
    {

        private readonly ISimulationUpdateManager _updateManager;
        private readonly ISimulationStateParameters _simStateParameters;
        private readonly NetworkTemplate _networkTemplate;
        private readonly SimulationRunner _simulationRunner;

        public MapManager(ISimulationStateParameters simStateParameters, NetworkTemplate networkTemplate, ISimulationUpdateManager updateManager)
        {
            _simStateParameters = simStateParameters;
            _networkTemplate = networkTemplate;
            _simulationRunner = new SimulationRunner(simStateParameters, updateManager);
            _updateManager = updateManager;
        }



        public void Run()
        {
            _updateManager.OnGeneration.Data.Generation = 0;
            _simulationRunner.InitializeAgents(_networkTemplate);
            RunSimulation();
        }

        private void RunSimulation()
        {
            do
            {
                _updateManager.ShouldUpdate = !_simStateParameters.RunInBackground;
                List<FitnessResults> results = new(_simulationRunner.GetFitnessResults());

                results.Sort();

                double avg = results.CalculateAverageResultOfTopPercent(.5) / _simStateParameters.NumberOfIterations;
                double avg25 = results.CalculateAverageResultOfTopPercent(.25) / _simStateParameters.NumberOfIterations;
                double avg10 = results.CalculateAverageResultOfTopPercent(.1) / _simStateParameters.NumberOfIterations;
                double avg1 = results.CalculateAverageResultOfTopPercent(.01) / _simStateParameters.NumberOfIterations;
                Debug.WriteLine($"G:{_simStateParameters.Generation} - 1%: {avg1:F4} - 10%: {avg10:F4} - 25%: {avg25:F4} - 50%: {avg:F4}");
                _simulationRunner.PropagateNewGeneration(results, _simStateParameters.Generation);

                _updateManager.OnGeneration.UpdateOnGeneration(avg);


                if (false)
                    return;

            } while (true);
        }
    }
}
