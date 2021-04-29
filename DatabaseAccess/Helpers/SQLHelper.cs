using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using DataAccessLibrary.DataAccessors.Network;
using DataAccessLibrary.DataTransferObjects;
using DataAccessLibrary.DataTransferObjects.NetworkDTOs;
using DataAccessLibrary.Internal;
using DataAccessLibrary.Internal.SQL;
using DataAccessLibrary.Internal.SQL.Enums;
using DataAccessLibrary.Internal.SQL.ParameterNames;

namespace DataAccessLibrary.Helpers
{
    public static class SQLHelper
    {

        public static void DoStuff()
        {
            CreateFiles(Table.NETWORK_BIAS, new NetworkBiasAccess(), new NetworkBiasDto());
            CreateFiles(Table.NETWORK_LAYER, new NetworkLayerAccess(), new NetworkLayerDto());
            CreateFiles(Table.NETWORK_WEIGHT, new NetworkWeightAccess(), new NetworkWeightDto());
        }


        static void CreateFiles<T>(Table table, SqlDatabaseAccessAbstract<T> access, T item)
            where T : SqlDataTransferObject
        {
            string path = Path.Combine(@"C:\Users\Kamil.Binko\Desktop\123", table.ToString());

            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);


            foreach ((string Data, string Name) file in GetFilesData(table, access, item))
            {
                var p = Path.Combine(path, file.Name) + ".sql";

                if(File.Exists(p))
                    File.Delete(p);

                using StreamWriter sw = new StreamWriter(File.Create(p));
                sw.Write(file.Data);
                sw.Close();
            }
        }


        private static IEnumerable<(string Data, string Name)> GetFilesData<T>(Table table, SqlDatabaseAccessAbstract<T> access, T item) where T : SqlDataTransferObject
        {
            yield return CreateType(table, access, item);
            yield return CreateGetAll(table, access, item);
            yield return CreateGetById(table, access, item);
            yield return CreateInsert(table, access, item);
            yield return CreateUpdate(table, access, item);
            yield return CreateUpsert(table, access, item);
            yield return CreateDelete(table, access, item);
            yield return Create_Insert_Many(table, access, item);
            yield return Create_Update_Many(table, access, item);
            yield return Create_Upsert_Many(table, access, item);
        }

        private static (string Data, string Name) CreateDelete<T>(Table table, SqlDatabaseAccessAbstract<T> access, T item) where T : SqlDataTransferObject
        {
            StringBuilder sb = new();
            sb.AppendLine($"CREATE PROCEDURE [dbo].[{table}_{Actions.DELETE_BY_ID}]");

            sb.AppendLine($"\t@{ParameterNames.cId} INT");

            sb.AppendLine("AS");

            sb.AppendLine($"\tDELETE FROM {table} WHERE {ParameterNames.cId} = @{ParameterNames.cId}");



            return (sb.ToString(), string.Join("_", table, Actions.DELETE_BY_ID));
        }

        private static (string Data, string Name) CreateUpdate<T>(Table table, SqlDatabaseAccessAbstract<T> access, T item) where T : SqlDataTransferObject
        {
            StringBuilder sb = new();
            BeginStoredProcedure(table, Actions.UPDATE, access, item, sb);

            Update(table, access, item, sb);

            return (sb.ToString(), string.Join("_", table, Actions.UPDATE));
        }

        private static (string Data, string Name) CreateUpsert<T>(Table table, SqlDatabaseAccessAbstract<T> access, T item) where T : SqlDataTransferObject
        {
            StringBuilder sb = new();
            BeginStoredProcedure(table, Actions.UPSERT, access, item, sb);
            sb.AppendLine("BEGIN");
            sb.AppendLine($"IF EXISTS(SELECT * FROM {table} WHERE {ParameterNames.cId}=@{ParameterNames.cId})");
            sb.AppendLine("BEGIN");

            Update(table, access, item, sb);

            sb.AppendLine("END");
            sb.AppendLine("ELSE");
            sb.AppendLine("BEGIN");

            Insert(table, access, item, sb);

            sb.AppendLine("END");

            sb.AppendLine("END");
            return (sb.ToString(), string.Join("_", table, Actions.UPSERT));
        }

        private static void Insert<T>(Table table, SqlDatabaseAccessAbstract<T> access, T item, StringBuilder sb)
            where T : SqlDataTransferObject
        {
            sb.AppendLine($"\tINSERT INTO {table} VALUES({GetParameterNames(access, item, false, "@")})");
            sb.AppendLine("\tSET @ID = SCOPE_IDENTITY();");
            sb.AppendLine("\tRETURN @ID");
        }

        private static void Update<T>(Table table, SqlDatabaseAccessAbstract<T> access, T item, StringBuilder sb)
            where T : SqlDataTransferObject
        {
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

            sb.AppendLine("\t" +string.Join(", ", GetStuff()));

            sb.AppendLine($"\tWHERE {ParameterNames.cId}=@{ParameterNames.cId}");
            sb.AppendLine("\tSET @ID = SCOPE_IDENTITY();");
            sb.AppendLine("\tRETURN @ID");
        }

        private static void BeginStoredProcedure<T>(Table table, Actions action, SqlDatabaseAccessAbstract<T> access,
            T item, StringBuilder sb)
            where T : SqlDataTransferObject
        {
            sb.AppendLine($"CREATE PROCEDURE [dbo].[{table}_{action}]");
            sb.AppendLine(GetParametrized(false, access, item));
            sb.AppendLine("AS");
        }

        private static (string Data, string Name) Create_Upsert_Many<T>(Table table, SqlDatabaseAccessAbstract<T> access, T item) where T : SqlDataTransferObject
        {
            StringBuilder sb = new();
            Many_Begin(table, sb, Actions.UPSERT_MANY);
            Update_Many(access, item, sb);
            Insert_Many(access, item, sb);
            Many_End(sb);


            return (sb.ToString(), string.Join("_", table, Actions.UPSERT_MANY));
        }

        private static (string Data, string Name) Create_Update_Many<T>(Table table, SqlDatabaseAccessAbstract<T> access, T item) where T : SqlDataTransferObject
        {
            StringBuilder sb = new();
            Many_Begin(table, sb, Actions.UPDATE_MANY);
            Update_Many(access, item, sb);

            Many_End(sb);


            return (sb.ToString(), string.Join("_", table, Actions.UPDATE_MANY));
        }

        private static void Many_End(StringBuilder sb)
        {
            sb.AppendLine(";");
            sb.AppendLine("END");
            sb.AppendLine("RETURN 0");
        }

        private static void Update_Many<T>(SqlDatabaseAccessAbstract<T> access, T item, StringBuilder sb)
            where T : SqlDataTransferObject
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

                    if (parameter.ParameterName == ParameterNames.cId)
                        continue;

                    yield return $"\t\t\tdbTable.{parameter.ParameterName} = tbl.{parameter.ParameterName}";
                }
            }

            sb.AppendLine( string.Join("," + Environment.NewLine, GetStuff()));

            sb.AppendLine();
        }

        private static (string Data, string Name) Create_Insert_Many<T>(Table table, SqlDatabaseAccessAbstract<T> access, T item) where T : SqlDataTransferObject
        {
            StringBuilder sb = new();
            Many_Begin(table, sb, Actions.INSERT_MANY);
            Insert_Many(access, item, sb);


            sb.AppendLine("END");
            sb.AppendLine("RETURN 0");


            return (sb.ToString(), string.Join("_", table, Actions.INSERT_MANY));
        }

        private static void Many_Begin(Table table, StringBuilder sb, Actions action)
        {
            sb.AppendLine($"CREATE PROCEDURE [dbo].[{table}_{action}]");
            sb.AppendLine($"\t@{ParameterNames.cDataTable} [dbo].{table}_TYPE READONLY");
            sb.AppendLine("AS");
            sb.AppendLine("BEGIN");
            sb.AppendLine($"\tMERGE [dbo].{table} as dbTable");
            sb.AppendLine($"\tUSING @{ParameterNames.cDataTable} as tbl");
            sb.AppendLine("\tON (dbTable.Id = tbl.Id)");
            sb.AppendLine();
        }

        private static void Insert_Many<T>(SqlDatabaseAccessAbstract<T> access, T item, StringBuilder sb)
            where T : SqlDataTransferObject
        {
            sb.AppendLine();
            sb.AppendLine("\tWHEN NOT MATCHED THEN");
            sb.AppendLine($"\t\tINSERT ({GetParameterNames(access, item, false, "[", "]")})");
            sb.AppendLine($"\t\tVALUES({GetParameterNames(access, item, false, "tbl.")});");
            sb.AppendLine();
        }

        private static (string Data, string Name) CreateInsert<T>(Table table, SqlDatabaseAccessAbstract<T> access, T item) where T : SqlDataTransferObject
        {
            StringBuilder sb = new();

            BeginStoredProcedure(table, Actions.INSERT, access, item, sb);
            sb.AppendLine("BEGIN");
            sb.AppendLine("\tSET NOCOUNT ON");
            sb.AppendLine("");
            Insert(table, access, item, sb);

            sb.AppendLine("END");



            return (sb.ToString(), string.Join("_", table, Actions.INSERT));
        }

        static string GetParameterNames<T>(SqlDatabaseAccessAbstract<T> access, T item, bool includeId, string prefix)
            where T : SqlDataTransferObject
        {
            return GetParameterNames(access, item, includeId, prefix, "");
        }
        static string GetParameterNames<T>(SqlDatabaseAccessAbstract<T> access, T item, bool includeId, string prefix, string suffix) where T : SqlDataTransferObject
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

        private static (string Data, string Name) CreateGetById<T>(Table table, SqlDatabaseAccessAbstract<T> access, T item) where T : SqlDataTransferObject
        {
            StringBuilder sb = new();

            sb.AppendLine($"CREATE PROCEDURE [dbo].[{table}_{Actions.GET_BY_ID}]");

            sb.AppendLine($"\t@{ParameterNames.cId} INT");

            sb.AppendLine("AS");
            sb.AppendLine($"\tSELECT * FROM [dbo].{table}");
            sb.AppendLine("\tWHERE Id = @ID");


            sb.AppendLine("RETURN 0");

            return (sb.ToString(), string.Join("_", table, Actions.GET_BY_ID));
        }

        private static (string Data, string Name) CreateGetAll<T>(Table table, SqlDatabaseAccessAbstract<T> access, T item) where T : SqlDataTransferObject
        {
            StringBuilder sb = new();

            sb.AppendLine($"CREATE PROCEDURE [dbo].[{table}_{Actions.GET_ALL}]");
            sb.AppendLine("AS");
            sb.AppendLine($"SELECT * FROM [dbo].{table}");

            return (sb.ToString(), string.Join("_", table, Actions.GET_ALL));
        }

        static IEnumerable<string> GetValues<T>(bool isType, SqlDatabaseAccessAbstract<T> access, T item) where T : SqlDataTransferObject
        {
            SqlCallParameters p = access.CreateDefaultParameters(item.ParametersCount, Actions.DELETE_BY_ID);
            SqlCallParameters parameters = item.CreateParameters(p);

            for (int i = 0; i < item.ParametersCount; i++)
            {
                SqlCallParameter parameter = parameters[i];

                if (isType)
                    yield return "\t" + string.Join(" ", parameter.ParameterName,
                        GetNameFromParameterType(parameter.DataType));
                else
                    yield return "\t" + string.Join(" ", "@" + parameter.ParameterName,
                        GetNameFromParameterType(parameter.DataType),
                        parameter.Direction == Direction.Output || parameter.Direction == Direction.InputOutput
                            ? "OUTPUT"
                            : "");


            }
        }

        static (string Data, string Name) CreateType<T>(Table table, SqlDatabaseAccessAbstract<T> access, T item) where T : SqlDataTransferObject
        {
            StringBuilder sb = new();

            sb.AppendLine($"CREATE TYPE {table}_TYPE as TABLE");
            sb.AppendLine("(");

            sb.AppendLine(GetParametrized(true, access, item));

            sb.AppendLine(")");

            return (sb.ToString(), string.Join("_", table, "TYPE"));
        }

        static string GetParametrized<T>(bool isType, SqlDatabaseAccessAbstract<T> access, T item)
            where T : SqlDataTransferObject
            => string.Join("," + Environment.NewLine, GetValues(isType, access, item));

        static string GetNameFromParameterType(DataType parameterDataType)
        {
            return parameterDataType switch
            {
                DataType.Int => "INT",
                DataType.String => "VARCHAR(MAX)",
                DataType.String25 => "VARCHAR(25)",
                DataType.String50 => "VARCHAR(50)",
                DataType.Double => "FLOAT",
                DataType.TimeStamp => "TIMESTAMP",
                DataType.DateTime => "DATETIME2",
                _ => throw new ArgumentOutOfRangeException(nameof(parameterDataType), parameterDataType, null)
            };
        }
    }
}
