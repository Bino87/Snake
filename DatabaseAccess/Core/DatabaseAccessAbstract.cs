using System.Threading.Tasks;

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
        public abstract int UpdateMany(T[] items);
        public abstract int UpsertMany(T[] items);
        public async Task<T[]> GetAllAsync() => await Task.Run(GetAll).ConfigureAwait(false);
        public async Task<T> GetByIdAsync(int id) => await Task.Run(() => GetById(id)).ConfigureAwait(false);
        public async Task<int> InsertAsync(T item) => await Task.Run(() => Insert(item)).ConfigureAwait(false);
        public async Task DeleteItemAsync(T item) => await Task.Run(() => DeleteItem(item)).ConfigureAwait(false);
        public async Task DeleteByIdAsync(int id) => await Task.Run(() => DeleteById(id)).ConfigureAwait(false);
        public async Task<int> UpdateAsync(T item) => await Task.Run(() => Update(item)).ConfigureAwait(false);
        public async Task<int> UpsertAsync(T item) => await Task.Run(() => Upsert(item)).ConfigureAwait(false);
        public async Task<int> InsertManyAsync(T[] items) => await Task.Run(() => InsertMany(items)).ConfigureAwait(false);
        public async Task<int> UpdateManyAsync(T[] items) => await Task.Run(() => UpdateMany(items)).ConfigureAwait(false);
        public async Task<int> UpsertManyAsync(T[] items) => await Task.Run(() => UpsertMany(items)).ConfigureAwait(false);
    }
}
