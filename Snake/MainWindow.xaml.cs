using Network;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
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
        private MainViewModel mvm ;
        public MainWindow()
        {
            InitializeComponent();
            mvm = new MainViewModel();
            this.DataContext = mvm;


            MapManager mm = new MapManager(mvm.SnakeMapViewModel._numberOfTiles, 200, Callback);
            Task.Run( ()=> mm.Run(Callback));
        }

        private void Callback(int arg1, MapCellStatus arg2)
        {
            Application.Current.Dispatcher.Invoke(() => {
                                                      mvm.SnakeMapViewModel.Rects[arg1].MapCellStatus = arg2;
                                                  });
            Thread.Sleep(100);
        }
    }
}
