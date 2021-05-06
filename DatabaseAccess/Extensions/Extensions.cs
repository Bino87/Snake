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

           

            sb.AppendLine();
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

            foreach (ColumnDefinition cd in dataTransferObjects[0].ColumnNames())
            {
                dt.Columns.Add(new DataColumn(cd.Name, cd.DataType.ToUnderlyingType()));
            }

            foreach (SqlDataTransferObject dto in dataTransferObjects)
            {
                DataRow row = dt.NewRow();

                dto.FillDataRow(row);

                dt.Rows.Add(row);
            }

            return dt;
        }
    }
}
