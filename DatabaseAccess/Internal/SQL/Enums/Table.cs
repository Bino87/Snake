using System;
using DataAccessLibrary.Internal.Enums;

namespace DataAccessLibrary.Internal.SQL.Enums
{
    public enum Table
    {
        TESTTABLE = 1,
        [TableAttribute(DatabaseType.Sql)]
        NETWORK_WEIGHT = 2,
        [TableAttribute(DatabaseType.Sql)]
        NETWORK_BIAS = 3,
        [TableAttribute(DatabaseType.Sql)]
        NETWORK_LAYER = 4,
        [TableAttribute(DatabaseType.MongoDB)]
        SIMULATION_GUI_PRESET = 5,
        [TableAttribute(DatabaseType.MongoDB)]
        NETWORK_TEMPLATE = 6,
        NETWORK_VALUE = 7,
    }

    internal  class TableAttribute : Attribute
    {
        internal DatabaseType DatabaseType { get; }

        internal TableAttribute(DatabaseType databaseType)
        {
            DatabaseType = databaseType;
        }
    }
}