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

        protected override void CreateStoredProcedureBody()
        {
            AppendLine($"\tDECLARE @RETURN_VALUE [dbo].{_table}_TYPE");
            AppendLine();
            AppendLine($"\tINSERT INTO [dbo].{_table}");
            AppendLine($"\tOUTPUT {GetParameterNames(true, "inserted.")} INTO @RETURN_VALUE");
            AppendLine($"\tSELECT {GetParameterNames(false, "")} FROM @{ParameterNames.SQL.cDataTable}");
            AppendLine();
            AppendLine("\tSELECT * FROM @RETURN_VALUE");
        }

        protected override void CreateParameters()
        {
            AppendLine($"\t@{ParameterNames.SQL.cDataTable} [dbo].{_table}_TYPE READONLY");
        }
    }
}