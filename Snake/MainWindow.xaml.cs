using System.Windows;
using DataAccessLibrary.DataAccessors.Network;
using DataAccessLibrary.DataTransferObjects.NetworkDTOs;
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
            //SQLHelper.CreateStoredProcedures();

            NetworkBiasAccess nba = new NetworkBiasAccess();

            NetworkBiasDto dto = new NetworkBiasDto();
            dto.LayerId = 1;
            dto.Value = 666.666;
            dto.InternalIndex = 1;
            nba.InsertMany(new[]
            {
                dto
            });
        }
    }
}
