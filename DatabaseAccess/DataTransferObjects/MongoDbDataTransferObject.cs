using System;
using DataAccessLibrary.Internal.Enums;
using DataAccessLibrary.Internal.MongoDB;

namespace DataAccessLibrary.DataTransferObjects
{
    public abstract class MongoDbDataTransferObject : DataTransferObject<Guid>
    {
        internal override DatabaseType DbType => DatabaseType.MongoDB;
        protected abstract int ParametersCount { get; }

        public virtual MongoDbCallParameters CreateParameters()
        {
            return new(ParametersCount);
        }
    }
}