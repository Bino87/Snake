using System;
using DataAccessLibrary.Internal.Enums;

namespace DataAccessLibrary.DataTransferObjects
{
    public class MongoDbDataTransferObject : DataTransferObject
    {
        internal Guid Id { get; }
        internal override DatabaseType DbType => DatabaseType.MongoDB;
    }
}