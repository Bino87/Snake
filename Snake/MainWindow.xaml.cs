using System.Windows;
using DataAccessLibrary.Helpers.SQL;
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
            DataContext = new MainDisplayHandlerViewModel();
            SQLHelper.CreateStoredProcedures();
        }
    }
}
