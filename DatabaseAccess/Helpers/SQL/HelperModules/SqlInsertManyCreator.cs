using DataAccessLibrary.DataTransferObjects;
using DataAccessLibrary.Extensions;
using DataAccessLibrary.Internal;
using DataAccessLibrary.Internal.SQL.Enums;
using DataAccessLibrary.Internal.SQL.ParameterNames;

namespace DataAccessLibrary.Helpers.SQL.HelperModules
{
    internal class SqlInsertManyCreator<T> : SqlStoredProcedureCreator<T> where T : SqlDataTransferObject
    {
        public SqlInsertManyCreator(SqlDatabaseAccessAbstract<T> access, T item, Table table) : base(access, item, table, Actions.INSERT_MANY)
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
            this.WhenNotMatched(sb, GetParameterNames);

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