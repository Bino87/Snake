using System.Data;
using System.Windows;
using UserControls.Models;
using DataAccessLibrary;
using DataAccessLibrary.DataTransferObjects;
using DataAccessLibrary.Internal;
using DataAccessLibrary.Internal.SQL.Enums;

namespace Snake
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly MainViewModel mvm;
        
       
        public MainWindow()
        {
            InitializeComponent();
            mvm = new MainViewModel();
            this.DataContext = mvm;
            Test t = new Test();

            var lol = t.Insert(new SqlDataTransferObject(123, 124));
        }

       
    }

    public class Test : SqlDatabaseAccessAbstract<SqlDataTransferObject>
    {
        protected override Table Table => Table.TESTTABLE;
        protected override SqlDataTransferObject CreateFromRow(DataRow dataTableRow)
        {
            return new SqlDataTransferObject(dataTableRow);
        }
    }

}
