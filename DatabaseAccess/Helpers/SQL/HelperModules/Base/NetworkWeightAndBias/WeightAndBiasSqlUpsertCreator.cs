using System.Collections.Generic;
using DataAccessLibrary.DataAccessors;
using DataAccessLibrary.DataTransferObjects;
using DataAccessLibrary.Internal.ParameterNames;
using DataAccessLibrary.Internal.SQL;
using DataAccessLibrary.Internal.SQL.Enums;

namespace DataAccessLibrary.Helpers.SQL.HelperModules.Base.NetworkWeightAndBias
{
    internal class WeightAndBiasSqlUpsertCreator<T> : DefaultSqlUpsertCreator<T> where T : SqlDataTransferObject
    {
        public WeightAndBiasSqlUpsertCreator(SqlDatabaseAccessAbstract<T> access, T item, Table table) : base(access, item, table)
        {
        }

        protected override void CreateStoredProcedureBody()
        {

            const string valueId = ParameterNames.SQL.cValueId;
            const string id = ParameterNames.SQL.cId;
            const string value = ParameterNames.SQL.cValue;

            AppendLine($"DECLARE @{valueId} INT");
            AppendLine($"EXEC @{valueId} = [dbo].[NETWORK_VALUE_TRY_INSERT] @{value} = @{value}");

            AppendLine($"IF EXISTS(SELECT * FROM {_table} WHERE {id}=@{id})");
            AppendLine("BEGIN");

            IEnumerable<string> GetStuff()
            {
                SqlCallParameters p = _access.CreateDefaultParameters(_item.ParametersCount, Actions.DELETE_BY_ID);
                SqlCallParameters parameters = _item.CreateParameters(p);

                for (int i = 0; i < _item.ParametersCount; i++)
                {
                    SqlCallParameter parameter = parameters[i];

                    if (parameter.ParameterName == id)
                        continue;

                    if(parameter.ParameterName == ParameterNames.SQL.cValue)
                        yield return $"{valueId} = @{valueId}";
                    else
                        yield return $"{parameter.ParameterName} = @{parameter.ParameterName}";
                }
            }

            AppendLine($"\tUPDATE {_table} SET ");

            AppendLine("\t" + string.Join(", ", GetStuff()));

            AppendLine($"\tWHERE {id}=@{id}");
            AppendLine($"\tSET @{id} = SCOPE_IDENTITY();");
            AppendLine($"\tRETURN @{id}");

            AppendLine("END");
            AppendLine("ELSE");
            AppendLine("BEGIN");

            AppendLine($"\tINSERT INTO {_table} VALUES({GetParameterNames(false, "@")})");
            AppendLine($"\tSET @{id} = SCOPE_IDENTITY();");
            AppendLine($"\tRETURN @{id}");

            AppendLine("END");
        }
    }
}