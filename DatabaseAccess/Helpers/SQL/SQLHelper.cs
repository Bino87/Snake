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


            foreach ((string data, string name) in GetFilesData(table, access, item))
            {
                var p = Path.Combine(path, name) + ".sql";

                if (File.Exists(p))
                    File.Delete(p);

                using StreamWriter sw = new StreamWriter(File.Create(p));
                sw.Write(data);
                sw.Close();
            }
        }


        private static IEnumerable<CreatorResult> GetFilesData<T>(Table table, SqlDatabaseAccessAbstract<T> access, T item) where T : SqlDataTransferObject
        {
            yield return new SqlTypeCreator<T>(access, item, table).Create();
            yield return new SqlGetAllCreator<T>(access, item, table).Create();
            yield return new SqlGetByIdCreator<T>(access, item, table).Create();
            yield return new SqlInsertCreator<T>(access, item, table).Create();
            yield return new SqlUpdateCreator<T>(access, item, table).Create();
            yield return new SqlUpsertCreator<T>(access, item, table).Create();
            yield return new SqlDeleteCreator<T>(access, item, table).Create();
            yield return new SqlInsertManyCreator<T>(access, item, table).Create();
            yield return new SqlUpdateManyCreator<T>(access, item, table).Create();
            yield return new SqlUpsertManyCreator<T>(access, item, table).Create();
        }
    }
}
