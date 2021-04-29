using DataAccessLibrary.DataTransferObjects;
using DataAccessLibrary.Extensions;
using DataAccessLibrary.Internal;
using DataAccessLibrary.Internal.SQL.Enums;
using DataAccessLibrary.Internal.SQL.ParameterNames;

namespace DataAccessLibrary.Helpers.HelperModules
{
    internal class SqlUpdateManyCreator<T> : SqlStoredProcedureCreator<T> where T : SqlDataTransferObject
    {
        public SqlUpdateManyCreator(SqlDatabaseAccessAbstract<T> access, T item, Table table) : base(access, item, table, Actions.UPDATE_MANY)
        {
        }

        protected override void CreateBody()
        {
            sb.AppendLine("AS");
            sb.AppendLine("BEGIN");
            sb.AppendLine($"\tMERGE [dbo].{table} as dbTable");
            sb.AppendLine($"\tUSING @{ParameterNames.cDataTable} as tbl");
            sb.AppendLine("\tON (dbTable.Id = tbl.Id)");
            sb.AppendLine();
            this.WhenMatched(sb, access, item);

            sb.AppendLine(";");
            sb.AppendLine("END");
            sb.AppendLine("RETURN 0");
        }

        protected override void CreateParameters()
        {
            sb.AppendLine($"\t@{ParameterNames.cDataTable} [dbo].{table}_TYPE READONLY");
        }
    }
}