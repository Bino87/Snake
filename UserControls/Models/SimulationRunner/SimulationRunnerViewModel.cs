using System;
using System.Threading;
using System.Threading.Tasks;
using Network;
using Network.Enums;
using Simulation;
using UserControls.Core.Base;
using UserControls.Managers;
using UserControls.Models.NeuralNetDisplay;

namespace UserControls.Models.SimulationRunner
{
    public class SimulationRunnerViewModel : MainView
    {
        public SnakeMapViewModel SnakeMapViewModel { get; set; }
        public SimulationGuiViewModel SimulationGuiViewModel { get; set; }
        public NeuralNetDisplayViewModel NeuralNetDisplay { get; set; }
        public ProgressGraphViewModel ProgressGraph { get; set; }
        private readonly MapManager _mm;


        public SimulationRunnerViewModel()
        {
            SnakeMapViewModel = new SnakeMapViewModel();
            SimulationGuiViewModel = new(StartSimulation);
            ProgressGraph = new ProgressGraphViewModel();

            NetworkTemplate networkTemplate = new(
                new LayerInfo(ActivationFunctionType.Identity,  2 * 4 + 8 * 7 + 6 + 3),
                new LayerInfo(ActivationFunctionType.ReLu, 10),
                new LayerInfo(ActivationFunctionType.ReLu, 10),
                new LayerInfo(ActivationFunctionType.Sigmoid, 3));
            NeuralNetDisplay = new NeuralNetDisplayViewModel(networkTemplate);
            _mm = new MapManager(SimulationGuiViewModel, networkTemplate, new SimulationUpdateManager(NeuralNetDisplay, SimulationGuiViewModel, SnakeMapViewModel, ProgressGraph));


        }


        private void StartSimulation()
        {
            CancellationTokenSource source = new();
            CancellationToken tok = source.Token;
            _ = Task.Run(() =>
                                {
                                    try
                                    {
                                        _mm.Run();
                                    }
                                    catch (Exception exception)
                                    {
                                        source.Cancel();
                                    }
                                }, tok);
        }

    }
}
