using System.Collections.Generic;
using DataAccessLibrary.DataTransferObjects;
using DataAccessLibrary.Internal;
using DataAccessLibrary.Internal.SQL;
using DataAccessLibrary.Internal.SQL.Enums;
using DataAccessLibrary.Internal.SQL.ParameterNames;

namespace DataAccessLibrary.Helpers.HelperModules
{
    internal class SqlUpdateCreator<T> : SqlStoredProcedureCreator<T> where T : SqlDataTransferObject
    {
        public SqlUpdateCreator(SqlDatabaseAccessAbstract<T> access, T item, Table table) : base(access, item, table, Actions.UPDATE)
        {
        }

        protected override void CreateBody()
        {
            sb.AppendLine("AS");

            IEnumerable<string> GetStuff()
            {
                SqlCallParameters p = access.CreateDefaultParameters(item.ParametersCount, Actions.DELETE_BY_ID);
                SqlCallParameters parameters = item.CreateParameters(p);

                for (int i = 0; i < item.ParametersCount; i++)
                {
                    SqlCallParameter parameter = parameters[i];

                    if (parameter.ParameterName == ParameterNames.cId)
                        continue;

                    yield return $"{parameter.ParameterName} = @{parameter.ParameterName}";
                }
            }

            sb.AppendLine($"\tUPDATE {table} SET ");

            sb.AppendLine("\t" + string.Join(", ", GetStuff()));

            sb.AppendLine($"\tWHERE {ParameterNames.cId}=@{ParameterNames.cId}");
            sb.AppendLine("\tSET @ID = SCOPE_IDENTITY();");
            sb.AppendLine("\tRETURN @ID");
        }

        protected override void CreateParameters()
        {
            sb.AppendLine(GetParametrized(false));
        }
    }
}