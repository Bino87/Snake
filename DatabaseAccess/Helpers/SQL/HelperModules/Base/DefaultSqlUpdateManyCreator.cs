
using System;
using System.Collections.Generic;
using DataAccessLibrary.DataAccessors;
using DataAccessLibrary.DataTransferObjects;
using DataAccessLibrary.Internal.ParameterNames;
using DataAccessLibrary.Internal.SQL;
using DataAccessLibrary.Internal.SQL.Enums;

namespace DataAccessLibrary.Helpers.SQL.HelperModules.Base
{
    internal class DefaultSqlUpdateManyCreator<T> : SqlStoredProcedureCreator<T> where T : SqlDataTransferObject
    {
        public DefaultSqlUpdateManyCreator(SqlDatabaseAccessAbstract<T> access, T item, Table table) : base(access, item, table, Actions.UPDATE_MANY)
        {
        }

        protected override void CreateStoredProcedureBody()
        {
            AppendLine();
            AppendLine("\tUPDATE AT");
            AppendLine($"\tSET {string.Join(",", GetStuff())}");
            AppendLine($"\tFROM {_table} tbl");
            AppendLine($"\tINNER JOIN @{ParameterNames.SQL.cDataTable} dt");
            AppendLine($"\t\tON tbl.ID = dt.ID");
            AppendLine();

            AppendLine(";");
            AppendLine("RETURN 0");
        }

        IEnumerable<string> GetStuff()
        {
            SqlCallParameters p = _access.CreateDefaultParameters(_item.ParametersCount, Actions.UPDATE_MANY);
            SqlCallParameters parameters = _item.CreateParameters(p);

            for (int i = 0; i < _item.ParametersCount; i++)
            {
                SqlCallParameter parameter = parameters[i];

                if (parameter.ParameterName == ParameterNames.SQL.cId)
                    continue;

                yield return $"\t{parameter.ParameterName} = dt.{parameter.ParameterName}";
            }
        }


        protected override void CreateParameters()
        {
            AppendLine($"\t@{ParameterNames.SQL.cDataTable} [dbo].{_table}_TYPE READONLY");
        }
    }
}