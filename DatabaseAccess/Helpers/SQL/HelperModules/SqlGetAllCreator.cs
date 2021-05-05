using DataAccessLibrary.DataAccessors;
using DataAccessLibrary.DataTransferObjects;
using DataAccessLibrary.Internal.SQL.Enums;

namespace DataAccessLibrary.Helpers.SQL.HelperModules
{
    internal class SqlGetAllCreator<T> : SqlStoredProcedureCreator<T> where T : SqlDataTransferObject
    {
        public SqlGetAllCreator(SqlDatabaseAccessAbstract<T> access, T item, Table table) : base(access, item, table, Actions.GET_ALL)
        {
        }

        protected override void CreateBody()
        {
            _sb.AppendLine("AS");
            _sb.AppendLine($"SELECT * FROM [dbo].{_table}");
        }

        protected override void CreateParameters()
        {
            
        }
    }
}