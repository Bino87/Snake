using DataAccessLibrary.DataAccessors;
using DataAccessLibrary.DataTransferObjects;
using DataAccessLibrary.Internal.SQL;
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
            return new(_sb.ToString(), string.Join("_", _table, "TYPE"));
        }

        protected override string GetValue(SqlCallParameter parameter)
        {
            return "\t" + string.Join(" ", parameter.ParameterName, GetNameFromParameterType(parameter.DataType));
        }

        protected override void CreateName()
        {
            _sb.AppendLine($"CREATE TYPE {_table}_TYPE as TABLE");
        }

        protected override void CreateBody()
        {
            _sb.AppendLine("(");

            _sb.AppendLine(GetParametrized());

            _sb.AppendLine(")");
        }

    }
}