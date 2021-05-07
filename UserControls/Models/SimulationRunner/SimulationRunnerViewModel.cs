using System;
using System.Threading;
using System.Threading.Tasks;
using Network;
using Network.Enums;
using Simulation;
using UserControls.Core.Base;
using UserControls.Core.Commands.MainViewCommands.SimulationRunnerCommands;
using UserControls.Managers;
using UserControls.Models.NeuralNetDisplay;

namespace UserControls.Models.SimulationRunner
{
    public class SimulationRunnerViewModel : MainView
    {
        CancellationTokenSource _source;
        private readonly MapManager _mm;

        public SnakeMapViewModel SnakeMapViewModel { get; set; }
        public SimulationGuiViewModel SimulationGuiViewModel { get; set; }
        public NeuralNetDisplayViewModel NeuralNetDisplay { get; set; }
        public ProgressGraphViewModel ProgressGraph { get; set; }

        public SimulationRunnerViewModel()
        {
            SnakeMapViewModel = new SnakeMapViewModel();
            SimulationGuiViewModel = new SimulationGuiViewModel(new StartSimulationCommand(StartSimulation, this), new StopSimulationCommand(Abort, this));
            ProgressGraph = new ProgressGraphViewModel();

            NetworkTemplate networkTemplate = new(
                new LayerInfo(ActivationFunctionType.Identity, 2 * 4 + 8 * 7 + 6 + 3),
                new LayerInfo(ActivationFunctionType.ReLu, 10),
                new LayerInfo(ActivationFunctionType.ReLu, 10),
                new LayerInfo(ActivationFunctionType.Sigmoid, 3));
            NeuralNetDisplay = new NeuralNetDisplayViewModel(networkTemplate);
            _mm = new MapManager(SimulationGuiViewModel, networkTemplate, new SimulationUpdateManager(NeuralNetDisplay, SimulationGuiViewModel, SnakeMapViewModel, ProgressGraph));
        }

        public override void Abort()
        {
            _source?.Cancel();
        }

        private void StartSimulation()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            _source = new CancellationTokenSource();
            CancellationToken tok = _source.Token;
            var t = Task.Run(() =>
                                {
                                    try
                                    {
                                        _mm.Run(tok);
                                        IsBusy = false;
                                        _source.Dispose();
                                    }
                                    catch (Exception exception)
                                    {
                                        _source.Cancel();
                                    }
                                }, tok);


        }
    }
}
