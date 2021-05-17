using DataAccessLibrary.DataAccessors;
using DataAccessLibrary.DataTransferObjects;
using DataAccessLibrary.Internal.ParameterNames;
using DataAccessLibrary.Internal.SQL.Enums;

namespace DataAccessLibrary.Helpers.SQL.HelperModules.Base
{
    internal class DefaultSqlDeleteCreator<T> : SqlStoredProcedureCreator<T> where T : SqlDataTransferObject
    {
        public DefaultSqlDeleteCreator(SqlDatabaseAccessAbstract<T> access, T item, Table table) : base(access, item, table, Actions.DELETE_BY_ID)
        {
        }

        protected override void CreateStoredProcedureBody()
        {
            AppendLine($"\tDELETE FROM {_table} WHERE {ParameterNames.SQL.cId} = @{ParameterNames.SQL.cId}");
        }

        protected override void CreateParameters()
        {
            AppendLine($"\t@{ParameterNames.SQL.cId} INT");
        }
    }
}