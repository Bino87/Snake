using DataAccessLibrary.DataTransferObjects;
using MongoDB.Driver;

namespace DataAccessLibrary.Internal.MongoDB
{
    internal record MongoDbCallParameter(string Name, object Value)
    {
        public UpdateDefinition<T> Set<T>(UpdateDefinitionBuilder<T> updateDefinitionBuilder, UpdateDefinition<T> def) where T : MongoDbDataTransferObject
        {
            return def is null ? updateDefinitionBuilder.Set(Name, Value) : def.Set(Name, Value);
        }
    }
}
