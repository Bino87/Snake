using DataAccessLibrary.DataTransferObjects;
using DataAccessLibrary.Extensions;
using DataAccessLibrary.Internal;
using DataAccessLibrary.Internal.SQL.Enums;
using DataAccessLibrary.Internal.SQL.ParameterNames;

namespace DataAccessLibrary.Helpers.SQL.HelperModules
{
    internal class SqlUpsertManyCreator<T> : SqlStoredProcedureCreator<T> where T : SqlDataTransferObject
    {
        public SqlUpsertManyCreator(SqlDatabaseAccessAbstract<T> access, T item, Table table) : base(access, item, table, Actions.UPSERT_MANY)
        {
        }

        protected override void CreateBody()
        {
            _sb.AppendLine("AS");
            _sb.AppendLine("BEGIN");
            _sb.AppendLine($"\tMERGE [dbo].{_table} as dbTable");
            _sb.AppendLine($"\tUSING @{ParameterNames.cDataTable} as tbl");
            _sb.AppendLine("\tON (dbTable.Id = tbl.Id)");
            _sb.AppendLine();
            this.WhenMatched(_sb, _access, _item);
            this.WhenNotMatched(_sb, GetParameterNames);

            _sb.AppendLine(";");
            _sb.AppendLine("END");
            _sb.AppendLine("RETURN 0");
        }

       

        protected override void CreateParameters()
        {
            _sb.AppendLine($"\t@{ParameterNames.cDataTable} [dbo].{_table}_TYPE READONLY");
        }
    }
}