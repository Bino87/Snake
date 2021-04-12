using Network;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using Network.ActivationFunctions;
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

            dudd += Dudd;
            dudd.Invoke(null, null);

        }

        private void Dudd(object sender, EventArgs e)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                mvm = new MainViewModel();
                this.DataContext = mvm;


                mm = new MapManager(Callback, 250, mvm.SnakeMapViewModel.RectArr, mvm.SnakeMapViewModel._numberOfTiles, 200);

                Task.Run(() =>
                {
                    //try
                    //{
                    mm.Run();

                    //}
                    //catch (Exception e)
                    //{

                    //}

                    dudd.Invoke(null, null);
                });
            });
        }

        private void Callback(int x, int y, MapCellStatus newStatus)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {

                if (x < 0 || x > mvm.SnakeMapViewModel._numberOfTiles || y < 0 || y > mvm.SnakeMapViewModel._numberOfTiles)
                {
                    //error
                    Console.Beep();
                }
                else
                {
                    mvm.SnakeMapViewModel.RectArr[x, y].CellStatus = newStatus;
                }
            });
            Thread.Sleep(mvm.Deley);
        }
    }
}
