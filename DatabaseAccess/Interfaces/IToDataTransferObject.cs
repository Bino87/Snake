using System;
using DataAccessLibrary.DataTransferObjects;

namespace DataAccessLibrary.Interfaces
{
    public interface IToSqlDataTransferObject<T> where T : SqlDataTransferObject
    {
        T ToDataTransferObject();
    }

    public interface IToMongoDbDataTransferObject<T> where T : MongoDbDataTransferObject
    {
        T ToDataTransferObject();
    }
}
