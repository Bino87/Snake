using System.Collections.Generic;
using System.Linq;
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
        private readonly NetworkInfo _networkInfo;
        private readonly SimulationRunner _simulationRunner;

        public MapManager(ISimulationStateParameters simStateParameters, NetworkInfo networkInfo, ISimulationUpdateManager updateManager)
        {
            _networkInfo = networkInfo;
            _simulationRunner = new SimulationRunner(simStateParameters, updateManager);
            _updateManager = updateManager;
        }



        public void Run()
        {
            _updateManager.OnGeneration.Data.Generation = 0;
            _simulationRunner.InitializeAgents(_networkInfo);
            RunSimulation();
        }

        private void RunSimulation()
        {
            do
            {
                List<FitnessResults> results = new(_simulationRunner.GetFitnessResults());

                results.Sort();

                double avg = results.CalculateAverageResult();

                _simulationRunner.PropagateNewGeneration(results);

                _updateManager.OnGeneration.UpdateOnGeneration(avg);


                if (false)
                    return;

            } while (true);
        }
    }
}
