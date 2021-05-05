using System.Collections.Generic;
using DataAccessLibrary.DataAccessors;
using DataAccessLibrary.DataTransferObjects;
using DataAccessLibrary.Internal.ParameterNames;
using DataAccessLibrary.Internal.SQL;
using DataAccessLibrary.Internal.SQL.Enums;


namespace DataAccessLibrary.Helpers.SQL.HelperModules
{
    internal class SqlUpsertCreator<T> : SqlStoredProcedureCreator<T> where T : SqlDataTransferObject
    {
        public SqlUpsertCreator(SqlDatabaseAccessAbstract<T> access, T item, Table table) : base(access, item, table, Actions.UPSERT)
        {
        }

        protected override void CreateBody()
        {
            _sb.AppendLine("AS");
            _sb.AppendLine("BEGIN");
            _sb.AppendLine($"IF EXISTS(SELECT * FROM {_table} WHERE {ParameterNames.SQL.cId}=@{ParameterNames.SQL.cId})");
            _sb.AppendLine("BEGIN");

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

            _sb.AppendLine($"\tUPDATE {_table} SET ");

            _sb.AppendLine("\t" + string.Join(", ", GetStuff()));

            _sb.AppendLine($"\tWHERE {ParameterNames.SQL.cId}=@{ParameterNames.SQL.cId}");
            _sb.AppendLine("\tSET @ID = SCOPE_IDENTITY();");
            _sb.AppendLine("\tRETURN @ID");

            _sb.AppendLine("END");
            _sb.AppendLine("ELSE");
            _sb.AppendLine("BEGIN");

            _sb.AppendLine($"\tINSERT INTO {_table} VALUES({GetParameterNames(false, "@")})");
            _sb.AppendLine("\tSET @ID = SCOPE_IDENTITY();");
            _sb.AppendLine("\tRETURN @ID");

            _sb.AppendLine("END");

            _sb.AppendLine("END");
        }

        protected override void CreateParameters()
        {
            _sb.AppendLine(GetParametrized());
        }
    }
}