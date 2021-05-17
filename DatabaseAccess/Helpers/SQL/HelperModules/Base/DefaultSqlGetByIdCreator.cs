using DataAccessLibrary.DataAccessors;
using DataAccessLibrary.DataTransferObjects;
using DataAccessLibrary.Internal.ParameterNames;
using DataAccessLibrary.Internal.SQL.Enums;


namespace DataAccessLibrary.Helpers.SQL.HelperModules.Base
{
    internal class DefaultSqlGetByIdCreator<T> : SqlStoredProcedureCreator<T> where T : SqlDataTransferObject
    {
        public DefaultSqlGetByIdCreator(SqlDatabaseAccessAbstract<T> access, T item, Table table) : base(access, item, table, Actions.GET_BY_ID)
        {
        }

        protected override void CreateStoredProcedureBody()
        {
            
            AppendLine($"\tSELECT * FROM [dbo].{_table}");
            AppendLine("\tWHERE Id = @ID");

            
        }

        protected override void CreateParameters()
        {
            AppendLine($"\t@{ParameterNames.SQL.cId} INT");
        }
    }
}