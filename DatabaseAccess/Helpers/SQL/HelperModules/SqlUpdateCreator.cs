using System.Collections.Generic;
using DataAccessLibrary.DataTransferObjects;
using DataAccessLibrary.Internal;
using DataAccessLibrary.Internal.SQL;
using DataAccessLibrary.Internal.SQL.Enums;
using DataAccessLibrary.Internal.SQL.ParameterNames;

namespace DataAccessLibrary.Helpers.SQL.HelperModules
{
    internal class SqlUpdateCreator<T> : SqlStoredProcedureCreator<T> where T : SqlDataTransferObject
    {
        public SqlUpdateCreator(SqlDatabaseAccessAbstract<T> access, T item, Table table) : base(access, item, table, Actions.UPDATE)
        {
        }

        protected override void CreateBody()
        {
            _sb.AppendLine("AS");

            IEnumerable<string> GetStuff()
            {
                SqlCallParameters p = _access.CreateDefaultParameters(_item.ParametersCount, Actions.DELETE_BY_ID);
                SqlCallParameters parameters = _item.CreateParameters(p);

                for (int i = 0; i < _item.ParametersCount; i++)
                {
                    SqlCallParameter parameter = parameters[i];

                    if (parameter.ParameterName == ParameterNames.cId)
                        continue;

                    yield return $"{parameter.ParameterName} = @{parameter.ParameterName}";
                }
            }

            _sb.AppendLine($"\tUPDATE {_table} SET ");

            _sb.AppendLine("\t" + string.Join(", ", GetStuff()));

            _sb.AppendLine($"\tWHERE {ParameterNames.cId}=@{ParameterNames.cId}");
            _sb.AppendLine("\tSET @ID = SCOPE_IDENTITY();");
            _sb.AppendLine("\tRETURN @ID");
        }

        protected override void CreateParameters()
        {
            _sb.AppendLine(GetParametrized());
        }
    }
}