using DataAccessLibrary.DataTransferObjects;
using DataAccessLibrary.Internal;
using DataAccessLibrary.Internal.SQL.Enums;

namespace DataAccessLibrary.Helpers.HelperModules
{
    internal class SqlInsertCreator<T> : SqlStoredProcedureCreator<T> where T : SqlDataTransferObject
    {
        public SqlInsertCreator(SqlDatabaseAccessAbstract<T> access, T item, Table table) : base(access, item, table, Actions.INSERT)
        {
        }

        protected override void CreateBody()
        {
            sb.AppendLine("AS");
            sb.AppendLine("BEGIN");
            sb.AppendLine("\tSET NOCOUNT ON");
            sb.AppendLine("");
            sb.AppendLine($"\tINSERT INTO {table} VALUES({GetParameterNames(false, "@")})");
            sb.AppendLine("\tSET @ID = SCOPE_IDENTITY();");
            sb.AppendLine("\tRETURN @ID");

            sb.AppendLine("END");
        }

        protected override void CreateParameters()
        {
            sb.AppendLine(GetParametrized(false));
        }
    }
}