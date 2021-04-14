using System;
using System.Collections.Generic;
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
            mm = new MapManager(OnUpdate, 50, SnakeMapViewModel.RectArr, SnakeMapViewModel._numberOfTiles, 50, ni, .05, 1);


            StartSimulation();
        }

        private void OnUpdate(List<(int X, int Y, MapCellStatus Status)> cellUpdateList, double[][] values, VisionData[] visionData)
        {

            Application.Current?.Dispatcher.Invoke(() =>
                                                  {
                                                      for (int i = 0; i < cellUpdateList.Count; i++)
                                                      {
                                                          (int x, int y, MapCellStatus status) = cellUpdateList[i];
                                                          SnakeMapViewModel.RectArr[x, y].CellStatus = status;
                                                      }

                                                      cellUpdateList.Clear();

                                                      if (values is not null)
                                                          NeuralNetDisplay.OnResultsCalculated(values);
                                                      if(visionData is not null)
                                                          SnakeMapViewModel.CreateVisionLines(visionData);

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
                                    catch (Exception)
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
