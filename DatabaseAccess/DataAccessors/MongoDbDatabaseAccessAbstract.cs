using System;
using DataAccessLibrary.Core;
using DataAccessLibrary.DataTransferObjects;
using DataAccessLibrary.Extensions;
using DataAccessLibrary.Internal.MongoDB;
using DataAccessLibrary.Internal.ParameterNames;
using MongoDB.Bson;
using MongoDB.Driver;


namespace DataAccessLibrary.DataAccessors
{
    public abstract class MongoDbDatabaseAccessAbstract<T> : DatabaseAccessAbstract<T, Guid> where T : MongoDbDataTransferObject
    {
        private const string cDataBase = "MongoDatabase";


        private IMongoCollection<T> _collection;

        private IMongoCollection<T> Collection
        {
            get
            {
                if (_collection is null)
                {
                    IMongoClient client = new MongoClient();
                    IMongoDatabase dataBase = client.GetDatabase(cDataBase);
                    _collection = dataBase.GetCollection<T>(Table.ToString());
                }

                return _collection;
            }
        }

        public override T[] GetAll()
        {
            return Collection.Find(new BsonDocument()).ToList().ToArray();
        }

        public override T GetById(Guid id)
        {
            FilterDefinition<T> filter = Builders<T>.Filter.Eq(ParameterNames.MongoDb.cId, id);

            return Collection.Find(filter).First();
        }

        public override Guid Insert(T item)
        {
            Collection.InsertOne(item);

            return item.Id;
        }

        public override void DeleteItem(T item)
        {
            FilterDefinition<T> filter = Builders<T>.Filter.Eq(ParameterNames.MongoDb.cId, item.Id);
            Collection.DeleteOne(filter);
        }

        public override void DeleteById(Guid id)
        {
            FilterDefinition<T> filter = Builders<T>.Filter.Eq(ParameterNames.MongoDb.cId, id);
            Collection.DeleteOne(filter);
        }

        public override Guid Update(T item)
        {
            FilterDefinition<T> filter = Builders<T>.Filter.Eq(ParameterNames.MongoDb.cId, item.Id);

            UpdateDefinition<T> def = item.GetUpdateDefinition<T>();

            UpdateResult res = Collection.UpdateOne(filter, def, new UpdateOptions { IsUpsert = true });

            return res.UpsertedId?.AsGuid ?? item.Id;
        }

        public abstract FilterDefinition<T> GetUpsertFilterDefinition(T item);

        public override Guid Upsert(T item)
        {
            FilterDefinition<T> filter = GetUpsertFilterDefinition(item);

            ReplaceOneResult res = Collection.ReplaceOne(filter, item, new ReplaceOptions { IsUpsert = true });

            return res.UpsertedId?.AsGuid ?? item.Id;
        }

        public override T[] InsertMany(T[] items)
        {
            Collection.InsertMany(items);

            return items;
        }

        public override void UpdateMany(T[] items, Guid id)
        {
            FilterDefinition<T> filter = Builders<T>.Filter.Eq(ParameterNames.MongoDb.cId, id);

            MongoDbCallParameters parameters = items[0].CreateParameters();

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


            Collection.UpdateMany(filter, def);
        }
    }
}
