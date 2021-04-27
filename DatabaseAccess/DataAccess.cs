using System;
using System.Collections.Generic;
using DatabaseAccess.Internal;
using DatabaseAccess.Core;
using DatabaseAccess.Internal.Enums;

namespace DatabaseAccess
{
    public class DataAccess
    {
        private Dictionary<DatabaseType, DatabaseAccessAbstract> _dataBaseAccessLookup;

        private static DataAccess instance;

        public static DataAccess Instance => instance ??= new DataAccess();

        public DataAccess()
        {
            _dataBaseAccessLookup = new Dictionary<DatabaseType, DatabaseAccessAbstract>();
        }

        private DatabaseAccessAbstract CreateDatabaseAccessInstance(DatabaseType dbType)
        {
            return dbType switch
            {
                DatabaseType.Sql => new SqlDatabaseAccessAbstract(),
                DatabaseType.MongoDB => new MongoDbDatabaseAccessAbstract(),
                _ => throw new ArgumentOutOfRangeException(nameof(dbType), dbType, null)
            };
        }
    }
}
