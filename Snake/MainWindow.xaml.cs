using Network;
using System;
using System.Windows;
using UserControls.Models;

namespace Snake
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = new MainViewModel();

            //Network.Network n = new(new Sigmoid(), 300, 200, 100);


            //Random rand = new Random();

            //double[] data = new double[300];

            //for (int i = 0; i < 300; i++)
            //{
            //    data[i] = rand.NextDouble(-10, 10);
            //}

            //var output = n.Evaluate(data);
        }
    }
}
