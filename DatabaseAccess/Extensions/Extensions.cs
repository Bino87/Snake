using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using DataAccessLibrary.DataAccessors;
using DataAccessLibrary.DataTransferObjects;
using DataAccessLibrary.Helpers.SQL.HelperModules;
using DataAccessLibrary.Internal.MongoDB;
using DataAccessLibrary.Internal.ParameterNames;
using DataAccessLibrary.Internal.SQL;
using DataAccessLibrary.Internal.SQL.Enums;
using MongoDB.Driver;

namespace DataAccessLibrary.Extensions
{
    internal static class Extensions
    {
        internal static void WhenMatched<T>(this SqlStoredProcedureCreator<T> val, StringBuilder sb, SqlDatabaseAccessAbstract<T> access, T item) where T : SqlDataTransferObject
        {
            sb.AppendLine("\tWHEN MATCHED THEN");
            sb.AppendLine("\t\tUPDATE SET  ");
            
            IEnumerable<string> GetStuff()
            {
                SqlCallParameters p = access.CreateDefaultParameters(item.ParametersCount, Actions.DELETE_BY_ID);
                SqlCallParameters parameters = item.CreateParameters(p);

                for (int i = 0; i < item.ParametersCount; i++)
                {
                    SqlCallParameter parameter = parameters[i];

                    if (parameter.ParameterName == ParameterNames.SQL.cId)
                        continue;

                    yield return $"\t\t\tdbTable.{parameter.ParameterName} = tbl.{parameter.ParameterName}";
                }
            }

            sb.AppendLine(string.Join(", " + Environment.NewLine, GetStuff()));

            sb.AppendLine();
            sb.AppendLine();
        }

        internal static void WhenNotMatched<T>(this SqlStoredProcedureCreator<T> val, StringBuilder sb, Func<bool, string, string, string> getParameterNames) where T : SqlDataTransferObject
        {
            sb.AppendLine("\tWHEN NOT MATCHED THEN");
            sb.AppendLine($"\t\tINSERT ({getParameterNames(false, "[", "]")})");
            sb.AppendLine($"\t\tVALUES({getParameterNames(false, "tbl.", "")});");
            sb.AppendLine();
        }

        internal static UpdateDefinition<T> GetUpdateDefinition<T>(this MongoDbDataTransferObject callParameters) where T : MongoDbDataTransferObject
        {
            MongoDbCallParameters parameters = callParameters.CreateParameters();

            UpdateDefinitionBuilder<T> updateDefinitionBuilder = Builders<T>.Update;
            UpdateDefinition<T> def = null;
            for (int i = 0; i < parameters.Count; i++)
            {
                def = parameters[i].Set(updateDefinitionBuilder, def);
            }

            return def;
        }

        internal static DataTable ToDataTable(this SqlDataTransferObject[] dataTransferObjects)
        {
            DataTable dt = new();

            foreach (string columnName in dataTransferObjects[0].ColumnNames())
            {
                dt.Columns.Add(columnName);
            }

            foreach (SqlDataTransferObject dto in  dataTransferObjects)
            {
                DataRow row = dt.NewRow();

                dto.FillDataRow(row);

                dt.Rows.Add(row);
            }

            return dt;
        }
    }
}
