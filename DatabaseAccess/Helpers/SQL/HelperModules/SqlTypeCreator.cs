using DataAccessLibrary.DataTransferObjects;
using DataAccessLibrary.Internal;
using DataAccessLibrary.Internal.SQL.Enums;

namespace DataAccessLibrary.Helpers.SQL.HelperModules
{
    internal class SqlTypeCreator<T> : SqlCreator<T> where T : SqlDataTransferObject
    {
        public SqlTypeCreator(SqlDatabaseAccessAbstract<T> access, T item, Table table) : base(access, item, table)
        {
        }

        protected override CreatorResult Return()
        {
            return new(sb.ToString(), string.Join("_", table, "TYPE"));
        }

        protected override void CreateName()
        {
            sb.AppendLine($"CREATE TYPE {table}_TYPE as TABLE");
        }

        protected override void CreateBody()
        {
            sb.AppendLine("(");

            sb.AppendLine(GetParametrized(true));

            sb.AppendLine(")");
        }

    }
}