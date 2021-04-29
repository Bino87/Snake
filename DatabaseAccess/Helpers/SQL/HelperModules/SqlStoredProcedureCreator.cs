using System.Text;
using DataAccessLibrary.DataTransferObjects;
using DataAccessLibrary.Internal;
using DataAccessLibrary.Internal.SQL;
using DataAccessLibrary.Internal.SQL.Enums;
using DataAccessLibrary.Internal.SQL.ParameterNames;

namespace DataAccessLibrary.Helpers.SQL.HelperModules
{
    internal abstract class SqlStoredProcedureCreator<T> : SqlCreator<T> where T : SqlDataTransferObject
    {
        protected Actions action;
        protected SqlStoredProcedureCreator(SqlDatabaseAccessAbstract<T> access, T item, Table table, Actions action) : base(access, item, table)
        {
            this.action = action;
        }

        protected override void CreateName()
        {
            sb.AppendLine($"CREATE PROCEDURE [dbo].[{table}_{action}]");
            CreateParameters();
        }

        protected abstract void CreateParameters();
        protected override CreatorResult Return()
        {
            return new(sb.ToString(), string.Join("_", table, action));
        }

        protected string GetParameterNames(bool includeId, string prefix)
        {
            return GetParameterNames(includeId, prefix, "");
        }
        protected string GetParameterNames(bool includeId, string prefix, string suffix)
        {
            SqlCallParameters p = access.CreateDefaultParameters(item.ParametersCount, Actions.DELETE_BY_ID);
            SqlCallParameters parameters = item.CreateParameters(p);

            StringBuilder sb = new();

            for (int i = 0; i < item.ParametersCount; i++)
            {
                SqlCallParameter parameter = parameters[i];

                if (!includeId && parameter.ParameterName == ParameterNames.cId)
                    continue;

                if (sb.Length > 0)
                    sb.Append(", ");

                sb.Append(prefix + parameter.ParameterName + suffix);

            }

            return sb.ToString();
        }
    }
}