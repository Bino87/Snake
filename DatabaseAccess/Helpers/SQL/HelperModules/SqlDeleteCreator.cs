using DataAccessLibrary.DataTransferObjects;
using DataAccessLibrary.Internal;
using DataAccessLibrary.Internal.SQL.Enums;
using DataAccessLibrary.Internal.SQL.ParameterNames;

namespace DataAccessLibrary.Helpers.SQL.HelperModules
{
    internal class SqlDeleteCreator<T> : SqlStoredProcedureCreator<T> where T : SqlDataTransferObject
    {
        public SqlDeleteCreator(SqlDatabaseAccessAbstract<T> access, T item, Table table) : base(access, item, table, Actions.DELETE_BY_ID)
        {
        }

        protected override void CreateBody()
        {
            _sb.AppendLine("AS");

            _sb.AppendLine($"\tDELETE FROM {_table} WHERE {ParameterNames.cId} = @{ParameterNames.cId}");
        }

        protected override void CreateParameters()
        {
            _sb.AppendLine($"\t@{ParameterNames.cId} INT");
        }
    }
}