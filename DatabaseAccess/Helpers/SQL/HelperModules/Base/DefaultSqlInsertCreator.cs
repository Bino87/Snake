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

        protected override void CreateStoredProcedureBody()
        {
            
            AppendLine("\tSET NOCOUNT ON");
            AppendLine("");
            AppendLine($"\tINSERT INTO {_table} VALUES({GetParameterNames(false, "@")})");
            AppendLine("\tSET @ID = SCOPE_IDENTITY();");
            AppendLine("\tRETURN @ID");

           
        }

        protected override void CreateParameters()
        {
            AppendLine(GetParametrized());
        }
    }
}