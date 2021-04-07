using Network;
using System;
using System.Diagnostics;
using System.Linq;
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
        private MapManager mm;
        private EventHandler dudd;
        public MainWindow()
        {
            InitializeComponent();
            dudd += Dudd;
            dudd.Invoke(null,null);
            
        }

        private void Dudd(object? sender, EventArgs e)
        {
            Application.Current.Dispatcher.Invoke(() => {
                                                      mvm = new MainViewModel();
                                                      this.DataContext = mvm;


                                                      mm = new MapManager(mvm.SnakeMapViewModel._numberOfTiles, 200, Callback);

                                                      Task.Run(() => {
                                                                   try
                                                                   {
                                                                       var a = mm.Run(Callback);

                                                                       Debug.WriteLine(string.Join(": ",a.Points,a.Moves.Min(), a.Moves.Max(), a.Moves.Average()));
                                                                   } catch(Exception)
                                                                   {

                                                                   }

                                                                   dudd.Invoke(null, null);
                                                               });
                                                  });
        }

        private void Callback(int arg1, MapCellStatus arg2)
        {
            Application.Current.Dispatcher.Invoke(() => {
                                                      mvm.SnakeMapViewModel.Rects[arg1].MapCellStatus = arg2;
                                                  });
            Thread.Sleep(10);
        }
    }
}
