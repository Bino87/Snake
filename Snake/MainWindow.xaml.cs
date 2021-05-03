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

            test t = new test() {Test = "DUDDDDDD123"};
            testDb tdb = new testDb();

            tdb.Upsert(t);
            t.Test = "DDDDadadadDDUD123";
            tdb.Upsert(t);
        }
    }

    class testDb : MongoDbDatabaseAccessAbstract<test>
    {
        protected override Table Table => Table.TESTTABLE;
    }

    class test : MongoDbDataTransferObject
    {
        public string Test { get; set; }
        protected override int ParametersCount => base.ParametersCount + 1;
        public override MongoDbCallParameters CreateParameters()
        {
            return base.CreateParameters().Add("Test", Test);
        }
    }
}
