

using System.Collections.Generic;
using DataAccessLibrary.DataAccessors;
using DataAccessLibrary.DataTransferObjects;
using DataAccessLibrary.Internal.ParameterNames;
using DataAccessLibrary.Internal.SQL;
using DataAccessLibrary.Internal.SQL.Enums;

namespace DataAccessLibrary.Helpers.SQL.HelperModules.Base
{
    internal class DefaultSqlUpdateCreator<T> : SqlStoredProcedureCreator<T> where T : SqlDataTransferObject
    {
        public DefaultSqlUpdateCreator(SqlDatabaseAccessAbstract<T> access, T item, Table table) : base(access, item, table, Actions.UPDATE)
        {
        }

        protected override void CreateStoredProcedureBody()
        {
          

            IEnumerable<string> GetStuff()
            {
                SqlCallParameters p = _access.CreateDefaultParameters(_item.ParametersCount, Actions.DELETE_BY_ID);
                SqlCallParameters parameters = _item.CreateParameters(p);

                for (int i = 0; i < _item.ParametersCount; i++)
                {
                    SqlCallParameter parameter = parameters[i];

                    if (parameter.ParameterName == ParameterNames.SQL.cId)
                        continue;

                    yield return $"{parameter.ParameterName} = @{parameter.ParameterName}";
                }
            }

            AppendLine($"\tUPDATE {_table} SET ");

            AppendLine("\t" + string.Join(", ", GetStuff()));

            AppendLine($"\tWHERE {ParameterNames.SQL.cId}=@{ParameterNames.SQL.cId}");
            AppendLine("\tSET @ID = SCOPE_IDENTITY();");
            AppendLine("\tRETURN @ID");
        }

        protected override void CreateParameters()
        {
            AppendLine(GetParametrized());
        }
    }
}