using DataAccessLibrary.Core;
using DataAccessLibrary.DataTransferObjects;

namespace DataAccessLibrary.Internal
{
    internal abstract class MongoDbDatabaseAccessAbstract<T> : DatabaseAccessAbstract<T> where T : MongoDbDataTransferObject
    {

    }
}
