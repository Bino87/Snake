using Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

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

            Network.Network n = new(new Sigmoid(), 300, 200, 100);


            Random rand = new Random();

            double[] data = new double[300];

            for (int i = 0; i < 300; i++)
            {
                data[i] = rand.NextDouble(-10, 10);
            }

            var output = n.Evaluate(data);
        }
    }
}
