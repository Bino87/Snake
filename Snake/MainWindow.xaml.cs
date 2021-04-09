using Network;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using Network.Mutators;
using Simulation;
using Simulation.Enums;
using UserControls.Models;

namespace Snake
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MainViewModel mvm;
        private MapManager mm;
        private EventHandler dudd;
        public MainWindow()
        {
            InitializeComponent();

            NeuralNetwork n1 = new NeuralNetwork(new ReLu(), 3, 2, 1);
            NeuralNetwork n2 = new NeuralNetwork(new ReLu(), 3, 2, 1);
            BitMutator bnm = new BitMutator();

            (NeuralNetwork First, NeuralNetwork Second) off = bnm.GetOffspring(n1, n2);

            dudd += Dudd;
            dudd.Invoke(null, null);

        }

        private void Dudd(object? sender, EventArgs e)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                mvm = new MainViewModel();
                this.DataContext = mvm;


                mm = new MapManager(mvm.SnakeMapViewModel.RectArr,mvm.SnakeMapViewModel._numberOfTiles, 200);

                Task.Run(() =>
                {
                    try
                    {
                        SimulationResult a = mm.Run(Callback);

                        Debug.WriteLine(a.Points);
                    }
                    catch (Exception e)
                    {

                    }

                    dudd.Invoke(null, null);
                });
            });
        }

        private void Callback((int X, int Y) tpl, MapCellStatus newStatus)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {

                if (tpl.X < 0 || tpl.X > mvm.SnakeMapViewModel._numberOfTiles || tpl.Y < 0 || tpl.Y > mvm.SnakeMapViewModel._numberOfTiles)
                {

                }
                else
                {
                    mvm.SnakeMapViewModel.RectArr[tpl.X, tpl.Y].CellStatus = newStatus;
                }




            });
            Thread.Sleep(10);
        }
    }
}
