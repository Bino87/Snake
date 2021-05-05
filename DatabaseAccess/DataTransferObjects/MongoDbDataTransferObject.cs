using System;
using DataAccessLibrary.Internal.Enums;
using DataAccessLibrary.Internal.MongoDB;

namespace DataAccessLibrary.DataTransferObjects
{
    public abstract class MongoDbDataTransferObject : DataTransferObject<Guid>
    {
        internal override DatabaseType DbType => DatabaseType.MongoDB;
        protected virtual int ParametersCount => 0;

        public MongoDbCallParameters CreateParameters()
        {
            return new(ParametersCount);
        }
    }
}