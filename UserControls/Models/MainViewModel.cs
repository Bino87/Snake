using System;
using System.Threading;
using System.Threading.Tasks;
using Network;
using Network.ActivationFunctions;
using Simulation;
using UserControls.Core.Base;
using UserControls.Managers;

namespace UserControls.Models
{
    public class MainViewModel : Observable
    {
        public SnakeMapViewModel SnakeMapViewModel { get; set; }
        public SimulationGuiViewModel SimulationGuiViewModel { get; set; }
        public NeuralNetDisplayViewModel NeuralNetDisplay { get; set; }
        public ProgressGraphViewModel ProgressGraph { get; set; }
        private readonly MapManager _mm;
        

        public MainViewModel()
        {
            SnakeMapViewModel = new SnakeMapViewModel();
            SimulationGuiViewModel = new(StartSimulation);
            ProgressGraph = new ProgressGraphViewModel();
            
            NetworkInfo ni = new(
                new LayerInfo(new Identity(), 2 * 4 + 8 * 7 + 6 + 3),
                new LayerInfo(new ReLu(), 30),
                new LayerInfo(new Sigmoid(), 15),
                new LayerInfo(new ReLu(), 9),
                new LayerInfo(new Sigmoid(), 3));
            NeuralNetDisplay = new NeuralNetDisplayViewModel(ni);
            _mm = new MapManager(SimulationGuiViewModel, ni, new SimulationUpdateManager(NeuralNetDisplay, SimulationGuiViewModel, SnakeMapViewModel, ProgressGraph));

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
