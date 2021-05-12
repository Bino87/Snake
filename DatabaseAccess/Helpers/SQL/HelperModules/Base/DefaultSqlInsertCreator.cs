using DataAccessLibrary.DataAccessors;
using DataAccessLibrary.DataTransferObjects;
using DataAccessLibrary.Internal.SQL.Enums;

namespace DataAccessLibrary.Helpers.SQL.HelperModules.Base
{
    internal class DefaultSqlInsertCreator<T> : SqlStoredProcedureCreator<T> where T : SqlDataTransferObject
    {
        public DefaultSqlInsertCreator(SqlDatabaseAccessAbstract<T> access, T item, Table table) : base(access, item, table, Actions.INSERT)
        {
        }

        protected override void CreateBody()
        {
            _sb.AppendLine("AS");
            _sb.AppendLine("BEGIN");
            _sb.AppendLine("\tSET NOCOUNT ON");
            _sb.AppendLine("");
            _sb.AppendLine($"\tINSERT INTO {_table} VALUES({GetParameterNames(false, "@")})");
            _sb.AppendLine("\tSET @ID = SCOPE_IDENTITY();");
            _sb.AppendLine("\tRETURN @ID");

            _sb.AppendLine("END");
        }

        protected override void CreateParameters()
        {
            _sb.AppendLine(GetParametrized());
        }
    }
}