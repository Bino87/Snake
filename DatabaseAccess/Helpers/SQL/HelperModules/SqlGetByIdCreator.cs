using DataAccessLibrary.DataTransferObjects;
using DataAccessLibrary.Internal;
using DataAccessLibrary.Internal.SQL.Enums;
using DataAccessLibrary.Internal.SQL.ParameterNames;

namespace DataAccessLibrary.Helpers.SQL.HelperModules
{
    internal class SqlGetByIdCreator<T> : SqlStoredProcedureCreator<T> where T : SqlDataTransferObject
    {
        public SqlGetByIdCreator(SqlDatabaseAccessAbstract<T> access, T item, Table table) : base(access, item, table, Actions.GET_BY_ID)
        {
        }

        protected override void CreateBody()
        {
            _sb.AppendLine("AS");
            _sb.AppendLine($"\tSELECT * FROM [dbo].{_table}");
            _sb.AppendLine("\tWHERE Id = @ID");

            _sb.AppendLine("RETURN 0");
        }

        protected override void CreateParameters()
        {
            _sb.AppendLine($"\t@{ParameterNames.cId} INT");
        }
    }
}