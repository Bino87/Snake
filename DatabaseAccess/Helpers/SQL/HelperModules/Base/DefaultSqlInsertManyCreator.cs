using DataAccessLibrary.DataAccessors;
using DataAccessLibrary.DataTransferObjects;
using DataAccessLibrary.Internal.ParameterNames;
using DataAccessLibrary.Internal.SQL.Enums;

namespace DataAccessLibrary.Helpers.SQL.HelperModules.Base
{
    internal class DefaultSqlInsertManyCreator<T> : SqlStoredProcedureCreator<T> where T : SqlDataTransferObject
    {
        public DefaultSqlInsertManyCreator(SqlDatabaseAccessAbstract<T> access, T item, Table table) : base(access, item, table, Actions.INSERT_MANY)
        {
        }

        protected override void CreateBody()
        {
            _sb.AppendLine("AS");
            _sb.AppendLine("BEGIN");

            _sb.AppendLine($"\tDECLARE @RETURN_VALUE [dbo].{_table}_TYPE");
            _sb.AppendLine();
            _sb.AppendLine($"\tINSERT INTO [dbo].{_table}");
            _sb.AppendLine($"\tOUTPUT {GetParameterNames(true, "inserted.")} INTO @RETURN_VALUE");
            _sb.AppendLine($"\tSELECT {GetParameterNames(false, "")} FROM @{ParameterNames.SQL.cDataTable}");
            _sb.AppendLine();
            _sb.AppendLine("\tSELECT * FROM @RETURN_VALUE");

            _sb.AppendLine(";");

            _sb.AppendLine("END");
            _sb.AppendLine("RETURN 0");
        }

        protected override void CreateParameters()
        {
            _sb.AppendLine($"\t@{ParameterNames.SQL.cDataTable} [dbo].{_table}_TYPE READONLY");
        }
    }
}