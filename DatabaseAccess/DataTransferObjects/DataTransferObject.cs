using DataAccessLibrary.Internal.Enums;
using MongoDB.Bson.Serialization.Attributes;

namespace DataAccessLibrary.DataTransferObjects
{
    public abstract class DataTransferObject<T>
    {
        [BsonId]
        public T Id { get; set; }
        [BsonIgnore]
        internal abstract DatabaseType DbType { get; }
    }
}
