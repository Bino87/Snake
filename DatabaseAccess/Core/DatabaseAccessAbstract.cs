using System.Threading.Tasks;
using DataAccessLibrary.DataTransferObjects;

namespace DataAccessLibrary.Core
{
    public abstract class DatabaseAccessAbstract<T>
    {
        public abstract T[] GetAll();

        public abstract T GetById(int id);

        public abstract int Insert(T item);

        public abstract void DeleteItem(T item);

        public abstract void DeleteById(int id);

        public abstract int Update(T item);

        public abstract int Upsert(T item);

        public abstract int InsertMany(T[] items);

        public abstract Task<T[]> GetAllAsync();

        public abstract Task<T> GetByIdAsync(int id);

        public abstract Task<int> InsertAsync(T item);

        public abstract Task DeleteItemAsync(T item);

        public abstract Task DeleteByIdAsync(int id);

        public abstract Task<int> UpdateAsync(T item);

        public abstract Task<int> UpsertAsync(T item);
    }
}
