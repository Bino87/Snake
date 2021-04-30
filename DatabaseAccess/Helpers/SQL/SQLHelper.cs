using System;
using System.Collections.Generic;
using System.IO;
using DataAccessLibrary.DataAccessors.Network;
using DataAccessLibrary.DataTransferObjects;
using DataAccessLibrary.DataTransferObjects.NetworkDTOs;
using DataAccessLibrary.Helpers.SQL.HelperModules;
using DataAccessLibrary.Internal;
using DataAccessLibrary.Internal.SQL.Enums;

namespace DataAccessLibrary.Helpers.SQL
{
    public class SQLHelper
    {

        public static void CreateStoredProcedures()
        {
            Table[] values = Enum.GetValues<Table>();

            foreach (Table table in values)
            {
                CreateFiles(table);
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


            foreach (SqlCreator <T> creator in GetFilesData(table, access, item))
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
            yield return new SqlTypeCreator<T>(access, item, table);
            yield return new SqlGetAllCreator<T>(access, item, table);
            yield return new SqlGetByIdCreator<T>(access, item, table);
            yield return new SqlInsertCreator<T>(access, item, table);
            yield return new SqlUpdateCreator<T>(access, item, table);
            yield return new SqlUpsertCreator<T>(access, item, table);
            yield return new SqlDeleteCreator<T>(access, item, table);
            yield return new SqlInsertManyCreator<T>(access, item, table);
            yield return new SqlUpdateManyCreator<T>(access, item, table);
            yield return new SqlUpsertManyCreator<T>(access, item, table);
        }
    }
}
