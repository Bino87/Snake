using System.Windows;
using DataAccessLibrary.DataTransferObjects;
using DataAccessLibrary.Internal;
using DataAccessLibrary.Internal.MongoDB;
using DataAccessLibrary.Internal.SQL.Enums;
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
            DataContext = new MainViewModel();
        }
    }
}
