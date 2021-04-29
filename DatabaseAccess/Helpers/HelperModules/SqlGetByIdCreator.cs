using DataAccessLibrary.DataTransferObjects;
using DataAccessLibrary.Internal;
using DataAccessLibrary.Internal.SQL.Enums;
using DataAccessLibrary.Internal.SQL.ParameterNames;

namespace DataAccessLibrary.Helpers.HelperModules
{
    internal class SqlGetByIdCreator<T> : SqlStoredProcedureCreator<T> where T : SqlDataTransferObject
    {
        public SqlGetByIdCreator(SqlDatabaseAccessAbstract<T> access, T item, Table table) : base(access, item, table, Actions.GET_BY_ID)
        {
        }

        protected override void CreateBody()
        {
            sb.AppendLine("AS");
            sb.AppendLine($"\tSELECT * FROM [dbo].{table}");
            sb.AppendLine("\tWHERE Id = @ID");

            sb.AppendLine("RETURN 0");
        }

        protected override void CreateParameters()
        {
            sb.AppendLine($"\t@{ParameterNames.cId} INT");
        }
    }
}