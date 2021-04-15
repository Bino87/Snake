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
using UserControls.Constants;
using UserControls.Core.Base;
using UserControls.Objects.SnakeDisplay;

namespace UserControls.Models
{
    public class MainViewModel : Observable
    {
        public SnakeMapViewModel SnakeMapViewModel { get; set; }
        public NeuralNetDisplayViewModel NeuralNetDisplay { get; set; }
        private int _delay;
        private MapManager mm;
        private int _generation;

        public int Generation
        {
            get => _generation;
            set => SetField(ref _generation, value);
        }

        public int Deley
        {
            get => _delay;
            set => SetField(ref _delay, value);

        }

        public MainViewModel()
        {
            SnakeMapViewModel = new SnakeMapViewModel(Cons.cNumberOfTiles);
            Deley = 500;
            NetworkInfo ni = new NetworkInfo(
                new LayerInfo(new Identity(), 2 * 4 + 8 * 3 + 6),
                new LayerInfo(new ReLu(), 20),
                new LayerInfo(new ReLu(), 12),
                new LayerInfo(new Sigmoid(), 3));
            NeuralNetDisplay = new NeuralNetDisplayViewModel(ni);
            mm = new MapManager(OnUpdate, 50, SnakeMapViewModel._numberOfTiles, 50, ni, .05, 1);


            StartSimulation();
        }

        private void OnUpdate(List<(int X, int Y, MapCellType Status)> cellUpdateList, double[][] values, VisionData[] visionData)
        {

            Application.Current?.Dispatcher.Invoke(() =>
            {
                SnakeMapViewModel.SetCells(cellUpdateList, visionData);

                if (values is not null)
                    NeuralNetDisplay.OnResultsCalculated(values);
            });
            DeleySim();
        }

        void DeleySim()
        {
            Thread.Sleep(Deley);
        }

        private void StartSimulation()
        {
            CancellationTokenSource source = new();
            CancellationToken tok = source.Token;
            _ = Task.Run(() =>
                                {
                                    try
                                    {
                                        mm.Run(OnSimulationStart, tok);
                                    }
                                    catch (Exception e)
                                    {
                                        source.Cancel();
                                    }
                                }, tok);
        }

        private void OnSimulationStart(double[][] obj, int generation)
        {
            Application.Current?.Dispatcher.Invoke(() =>
            {
                NeuralNetDisplay.UpdateWeights(obj);
                Generation = generation;
            }
            );
            DeleySim();
        }
    }
}
