using System.Collections.Generic;
using DataAccessLibrary.DataTransferObjects;
using DataAccessLibrary.Internal;
using DataAccessLibrary.Internal.SQL;
using DataAccessLibrary.Internal.SQL.Enums;
using DataAccessLibrary.Internal.SQL.ParameterNames;

namespace DataAccessLibrary.Helpers.HelperModules
{
    internal class SqlUpsertCreator<T> : SqlStoredProcedureCreator<T> where T : SqlDataTransferObject
    {
        public SqlUpsertCreator(SqlDatabaseAccessAbstract<T> access, T item, Table table) : base(access, item, table, Actions.UPSERT)
        {
        }

        protected override void CreateBody()
        {
            sb.AppendLine("AS");
            sb.AppendLine("BEGIN");
            sb.AppendLine($"IF EXISTS(SELECT * FROM {table} WHERE {ParameterNames.cId}=@{ParameterNames.cId})");
            sb.AppendLine("BEGIN");

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

            sb.AppendLine("END");
            sb.AppendLine("ELSE");
            sb.AppendLine("BEGIN");

            sb.AppendLine($"\tINSERT INTO {table} VALUES({GetParameterNames(false, "@")})");
            sb.AppendLine("\tSET @ID = SCOPE_IDENTITY();");
            sb.AppendLine("\tRETURN @ID");

            sb.AppendLine("END");

            sb.AppendLine("END");
        }

        protected override void CreateParameters()
        {
            sb.AppendLine(GetParametrized(false));
        }
    }
}