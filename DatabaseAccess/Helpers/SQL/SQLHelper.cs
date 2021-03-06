using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Commons.Extensions;
using DataAccessLibrary.DataAccessors;
using DataAccessLibrary.DataAccessors.Network;
using DataAccessLibrary.DataTransferObjects;
using DataAccessLibrary.DataTransferObjects.NetworkDTOs;
using DataAccessLibrary.Helpers.SQL.HelperModules.Base;
using DataAccessLibrary.Helpers.SQL.HelperModules.CreatorProviders;
using DataAccessLibrary.Internal.Enums;
using DataAccessLibrary.Internal.SQL.Enums;

namespace DataAccessLibrary.Helpers.SQL
{
    public class SQLHelper
    {

        public static void CreateStoredProcedures()
        {
            foreach (Table table in GetSqlTables())
            {
                CreateFiles(table);
            }
        }

        static IEnumerable<Table> GetSqlTables()
        {
            Table[] values = Enum.GetValues<Table>();

            foreach (Table table in values)
            {
                MemberInfo[] memberInfo = typeof(Table).GetMember(table.ToString());

                for(int i = 0; i < memberInfo.Length; i++)
                {
                    TableAttribute attribute = memberInfo[i].GetCustomAttribute<TableAttribute>();
                    if (attribute.IsNotNull() && attribute.DatabaseType == DatabaseType.Sql)
                    {
                        yield return table;
                        break;
                    }
                }

                
            }
        }

        private static void CreateFiles(Table table)
        {
            switch (table)
            {
                case Table.NETWORK_WEIGHT:
                    CreateFiles(Table.NETWORK_WEIGHT, new NetworkWeightAccess(), new NetworkWeightDto());
                    break;
                case Table.NETWORK_BIAS:
                    CreateFiles(Table.NETWORK_BIAS, new NetworkBiasAccess(), new NetworkBiasDto());
                    break;
                case Table.NETWORK_LAYER:
                    CreateFiles(Table.NETWORK_LAYER, new NetworkLayerAccess(), new NetworkLayerDto());
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(table), table, null);
            }
        }


        private static void CreateFiles<T>(Table table, SqlDatabaseAccessAbstract<T> access, T item) where T : SqlDataTransferObject
        {
            string path = Path.Combine(GetBasePath(), table.ToString());

            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);


            foreach (SqlCreator<T> creator in GetFilesData(table, access, item))
            {
                (string data, string name) = creator.Create();
                string p = Path.Combine(path, name) + ".sql";

                if (File.Exists(p))
                    File.Delete(p);

                using StreamWriter sw = new(File.Create(p));
                sw.Write(data);
                sw.Close();
            }
        }

        private static string GetBasePath() => Environment.MachineName switch
        {
            "DESKTOP-D40UEJC" => @"C:\Users\Kamil\Desktop\123",
            "DESKTOP-GIT7C0K" => @"C:\Users\Kamil.Binko\Desktop\123",
            _ => throw new Exception("Machine not recognized!")
        };



        private static IEnumerable<SqlCreator<T>> GetFilesData<T>(Table table, SqlDatabaseAccessAbstract<T> access, T item) where T : SqlDataTransferObject
        {

            ISqlCreatorProvider<T> provider = GetProvider(table, access, item);

            yield return provider.Type(); 
            yield return provider.GetAll();
            yield return provider.GetById();
            yield return provider.Insert();
            yield return provider.Update();
            yield return provider.Upsert();
            yield return provider.Delete();
            yield return provider.InsertMany();
            yield return provider.UpdateMany();
        }

        private static ISqlCreatorProvider<T> GetProvider<T>(Table table, SqlDatabaseAccessAbstract<T> access, T item) where T : SqlDataTransferObject  => table switch 
        {
            
            Table.NETWORK_BIAS => new WeightAndBiasSqlCreatorProvider<T>(table, access, item),
            Table.NETWORK_WEIGHT=> new WeightAndBiasSqlCreatorProvider<T>(table, access, item),
            _ => new SqlCreatorProvider<T>(table, access, item)
        };

    }
}
