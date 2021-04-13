using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using Network;
using Network.ActivationFunctions;
using Simulation;
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

        public int Deley
        {
            get => _delay;
            set => SetField(ref _delay, value);

        }

        public MainViewModel()
        {
            SnakeMapViewModel = new SnakeMapViewModel(Cons.cNumberOfTiles);
            Deley = 50;
            NetworkInfo ni = new NetworkInfo(
                new LayerInfo(new Identity(), 2 * 4 + 8 * 3 + 6),
                new LayerInfo(new LeakyRelu(), 20),
                new LayerInfo(new LeakyRelu(), 12),
                new LayerInfo(new Sigmoid(), 4));
            NeuralNetDisplay = new NeuralNetDisplayViewModel(ni);
            mm = new MapManager(Callback, NeuralNetDisplay.OnResultsCalculated, 10, SnakeMapViewModel.RectArr, SnakeMapViewModel._numberOfTiles, 200, ni);


            Dudd();
        }

        private void Dudd()
        {
            Application.Current.Dispatcher.Invoke(async () =>
                                                  {
                                                      await Task.Run(() =>
                                                                     {
                                                                         mm.Run(NeuralNetDisplay.OnUpdateWeights);
                                                                     });

                                                  });
        }

        private void Callback(int x, int y, MapCellStatus newStatus)
        {
            if (Application.Current?.Dispatcher is null)
                return;

            Application.Current.Dispatcher.Invoke(() =>
                                                  {

                                                      if (x < 0 || x > SnakeMapViewModel._numberOfTiles || y < 0 || y > SnakeMapViewModel._numberOfTiles)
                                                      {
                                                          //error
                                                          Console.Beep();
                                                      }
                                                      else
                                                      {
                                                          SnakeMapViewModel.RectArr[x, y].CellStatus = newStatus;
                                                      }
                                                  });
            Thread.Sleep(Deley);
        }
    }
}
