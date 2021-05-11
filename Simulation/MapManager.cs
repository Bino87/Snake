using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
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
        private  NetworkTemplate _networkTemplate;
        private SimulationRunner _simulationRunner;

        public MapManager(ISimulationStateParameters simStateParameters, ISimulationUpdateManager updateManager)
        {
            _simStateParameters = simStateParameters;
            _updateManager = updateManager;
        }

        public void SetTemplate(NetworkTemplate nt) => _networkTemplate = nt;


        public void Run(CancellationToken cancellationToken)
        {
            _updateManager.OnGeneration.Data.Generation = 0;
            _simulationRunner = new SimulationRunner(_simStateParameters, _updateManager);
            _simulationRunner.InitializeAgents(_networkTemplate);
            RunSimulation(cancellationToken);
        }

        private void RunSimulation(CancellationToken cancellationToken)
        {
            List<FitnessResults> results = null;
            Action[] actions = {
                () => { _updateManager.ShouldUpdate = !_simStateParameters.RunInBackground; },
                () => {results = new List<FitnessResults>(_simulationRunner.GetFitnessResults(cancellationToken));},
                () => { results.Sort(); },
                () => { _simulationRunner.PropagateNewGeneration(results, _simStateParameters.Generation); },
                () => { _updateManager.OnGeneration.UpdateOnGeneration(results.CalculateAverageResultOfTopPercent(.5) / _simStateParameters.NumberOfIterations); },
            };

            do
            {
                foreach (Action action in actions)
                {
                    action(); // not the best looking way  of doing stuff but it checks for cancelation all the time and thats the point. 
                    if (!cancellationToken.IsCancellationRequested) 
                        continue;
                    
                    break;
                }
            } while (!cancellationToken.IsCancellationRequested);
            
            _simulationRunner.Abort();
        }
    }
}
