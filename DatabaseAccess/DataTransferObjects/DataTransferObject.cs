﻿using DataAccessLibrary.Internal.Enums;
using MongoDB.Bson.Serialization.Attributes;

namespace DataAccessLibrary.DataTransferObjects
{
    public abstract class DataTransferObject<T>
    {
        [BsonId]
        //[BsonElement(ParameterNames.cSqlId)]
        internal T Id { get; set; }
        [BsonIgnore]
        internal abstract DatabaseType DbType { get; }
    }
}
