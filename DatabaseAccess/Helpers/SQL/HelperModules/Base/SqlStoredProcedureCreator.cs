
using System.Text;
using DataAccessLibrary.DataAccessors;
using DataAccessLibrary.DataTransferObjects;
using DataAccessLibrary.Internal.ParameterNames;
using DataAccessLibrary.Internal.SQL;
using DataAccessLibrary.Internal.SQL.Enums;

namespace DataAccessLibrary.Helpers.SQL.HelperModules.Base
{
    internal abstract class SqlStoredProcedureCreator<T> : SqlCreator<T> where T : SqlDataTransferObject
    {
        protected Actions _action;
        protected SqlStoredProcedureCreator(SqlDatabaseAccessAbstract<T> access, T item, Table table, Actions action) : base(access, item, table)
        {
            this._action = action;
        }

        protected override void CreateName()
        {
            _sb.AppendLine($"CREATE PROCEDURE [dbo].[{_table}_{_action}]");
            CreateParameters();
        }

        protected abstract void CreateParameters();
        protected override CreatorResult Return() => new(_sb.ToString(), string.Join("_", _table, _action));

        protected string GetParameterNames(bool includeId, string prefix) => GetParameterNames(includeId, prefix, "");

        protected string GetParameterNames(bool includeId, string prefix, string suffix)
        {
            SqlCallParameters p = _access.CreateDefaultParameters(_item.ParametersCount, Actions.DELETE_BY_ID);
            SqlCallParameters parameters = _item.CreateParameters(p);
            StringBuilder sb = new();

            for (int i = 0; i < _item.ParametersCount; i++)
            {
                SqlCallParameter parameter = parameters[i];

                if (!includeId && parameter.ParameterName == ParameterNames.SQL.cId)
                    continue;

                if (sb.Length > 0)
                    sb.Append(", ");

                sb.Append(prefix + parameter.ParameterName + suffix);
            }

            return sb.ToString();
        }

        protected override string GetValue(SqlCallParameter parameter)
        {
            if (parameter.Direction == Direction.Output || parameter.Direction == Direction.InputOutput)
                return "\t" + string.Join(" ", "@" + parameter.ParameterName, GetNameFromParameterType(parameter.DataType), "OUTPUT");
            return "\t" + string.Join(" ", "@" + parameter.ParameterName, GetNameFromParameterType(parameter.DataType));
        }
    }
}