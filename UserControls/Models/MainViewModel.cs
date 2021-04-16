using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using Network;
using Network.ActivationFunctions;
using Simulation;
using Simulation.Core;
using Simulation.Enums;
using UserControls.Core.Base;
using UserControls.Objects.SnakeDisplay;

namespace UserControls.Models
{
    public class MainViewModel : Observable
    {
        public SnakeMapViewModel SnakeMapViewModel { get; set; }
        public SimulationGuiViewModel SimulationGuiViewModel { get; set; }
        public NeuralNetDisplayViewModel NeuralNetDisplay { get; set; }
        private MapManager mm;
        

        public MainViewModel()
        {
            SnakeMapViewModel = new SnakeMapViewModel();
            SimulationGuiViewModel = new(StartSimulation);
            
            NetworkInfo ni = new(
                new LayerInfo(new Identity(), 2 * 4 + 8 * 5 + 6),
                new LayerInfo(new ReLu(), 20),
                new LayerInfo(new ReLu(), 12),
                new LayerInfo(new Sigmoid(), 5));
            NeuralNetDisplay = new NeuralNetDisplayViewModel(ni);
            mm = new MapManager(OnUpdate, SimulationGuiViewModel, ni);

        }

        private void OnUpdate(List<(int X, int Y, MapCellType Status)> cellUpdateList, double[][] values, VisionData[] visionData)
        {

            Application.Current?.Dispatcher.Invoke(() =>
            {
                SnakeMapViewModel.SetCells(cellUpdateList, visionData, SimulationGuiViewModel.MapSize);

                if (values is not null)
                    NeuralNetDisplay.OnResultsCalculated(values);
            });
            DeleySim();
        }

        void DeleySim()
        {
            Thread.Sleep(SimulationGuiViewModel.UpdateDelay);
        }

        private void StartSimulation()
        {
            CancellationTokenSource source = new();
            CancellationToken tok = source.Token;
            _ = Task.Run(() =>
                                {
                                    try
                                    {
                                        mm.Run(OnSimulationStart);
                                    }
                                    catch (Exception exception)
                                    {
                                        source.Cancel();
                                    }
                                }, tok);
        }

        private void OnSimulationStart(double[][] obj, int generation, int individual)
        {
            Application.Current?.Dispatcher.Invoke(() =>
            {
                NeuralNetDisplay.UpdateWeights(obj);
                SimulationGuiViewModel.Generation = generation;
            }
            );
            DeleySim();
        }
    }
}
