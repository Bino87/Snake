using System;
using DataAccessLibrary.Core;
using DataAccessLibrary.DataTransferObjects;
using DataAccessLibrary.Internal.MongoDB;
using DataAccessLibrary.Internal.SQL.ParameterNames;
using MongoDB.Bson;
using MongoDB.Driver;


namespace DataAccessLibrary.Internal
{
    public abstract class MongoDbDatabaseAccessAbstract<T> : DatabaseAccessAbstract<T, Guid> where T : MongoDbDataTransferObject
    {
        private const string cDataBase = "MongoDatabase";

        private readonly IMongoDatabase _database;
        private readonly IMongoCollection<T> _collection;

        protected MongoDbDatabaseAccessAbstract()
        {
            MongoClient client = new MongoClient();
            _database = client.GetDatabase(cDataBase);
            _collection = _database.GetCollection<T>(Table.ToString());
        }
        public override T[] GetAll()
        {
            return _collection.Find(new BsonDocument()).ToList().ToArray();
        }

        public override T GetById(Guid id)
        {
            FilterDefinition<T> filter = Builders<T>.Filter.Eq(ParameterNames.cId, id);

            return _collection.Find(filter).First();
        }

        public override Guid Insert(T item)
        {
            _collection.InsertOne(item);

            return item.Id;
        }

        public override void DeleteItem(T item)
        {
            FilterDefinition<T> filter = Builders<T>.Filter.Eq(ParameterNames.cId, item.Id);
            _collection.DeleteOne(filter);
        }

        public override void DeleteById(Guid id)
        {
            FilterDefinition<T> filter = Builders<T>.Filter.Eq(ParameterNames.cId, id);
            _collection.DeleteOne(filter);
        }

        public override Guid Update(T item)
        {
            FilterDefinition<T> filter = Builders<T>.Filter.Eq("Id", item.Id);

            MongoDbCallParameters parameters = item.CreateParameters();

            UpdateDefinitionBuilder<T> updateDefinitionBuilder = Builders<T>.Update;
            UpdateDefinition<T> def = null;
            for (int i = 0; i < parameters.Count; i++)
            {
                MongoDbCallParameter element = parameters[i];
                if (def is null)
                    def = updateDefinitionBuilder.Set(element.Name, element.Value);
                else
                    def.Set(element.Name, element.Value);
            }


            UpdateResult res =  _collection.UpdateOne(filter, def, new UpdateOptions());
            return item.Id;
        }

        public override Guid Upsert(T item)
        {
            return item.Id == default ? Insert(item) : Update(item);
        }

        public override Guid InsertMany(T[] items)
        {
            return default;
        }

        public override Guid UpdateMany(T[] items)
        {
            return default;
        }

        public override Guid UpsertMany(T[] items)
        {
            return default;
        }
    }
}
