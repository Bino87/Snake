
using DataAccessLibrary.DataAccessors;
using DataAccessLibrary.DataTransferObjects;
using DataAccessLibrary.Internal.SQL;
using DataAccessLibrary.Internal.SQL.Enums;

namespace DataAccessLibrary.Helpers.SQL.HelperModules.Base
{
    internal class DefaultSqlTypeCreator<T> : SqlCreator<T> where T : SqlDataTransferObject
    {
        public DefaultSqlTypeCreator(SqlDatabaseAccessAbstract<T> access, T item, Table table) : base(access, item, table)
        {
        }

        protected override CreatorResult Return()
        {
            return new(ToString(), string.Join("_", _table, "TYPE"));
        }

        protected override string GetValue(SqlCallParameter parameter)
        {
            return "\t" + string.Join(" ", parameter.ParameterName, GetNameFromParameterType(parameter.DataType));
        }

        protected override void CreateName()
        {
            AppendLine($"CREATE TYPE {_table}_TYPE as TABLE");
        }

        protected override void CreateBody()
        {
            AppendLine("(");

            AppendLine(GetParametrized());

            AppendLine(")");
        }

    }
}