using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
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

            NeuralNetDisplay = new NeuralNetDisplayViewModel();
            _mm = new MapManager(SimulationGuiViewModel, new SimulationUpdateManager(NeuralNetDisplay, SimulationGuiViewModel, SnakeMapViewModel, ProgressGraph));
        }

        public override void Abort()
        {
            _source?.Cancel();
        }

        private void StartSimulation()
        {
            if (IsBusy)
                return;

            if (SimulationGuiViewModel.SelectedTemplate is null)
            {
                MessageBox.Show("No Network Template is selected.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            _mm.SetTemplate(SimulationGuiViewModel.SelectedTemplate);
            NeuralNetDisplay.Initialize(SimulationGuiViewModel.SelectedTemplate);

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
