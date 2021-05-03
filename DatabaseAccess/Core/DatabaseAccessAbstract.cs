using System.Threading.Tasks;
using DataAccessLibrary.Internal.SQL.Enums;

namespace DataAccessLibrary.Core
{
    public abstract class DatabaseAccessAbstract<T,TID>
    {
        protected abstract Table Table { get; }
        public abstract T[] GetAll();
        public abstract T GetById(TID id);
        public abstract TID Insert(T item);
        public abstract void DeleteItem(T item);
        public abstract void DeleteById(TID id);
        public abstract TID Update(T item);
        public abstract TID Upsert(T item);
        public abstract void InsertMany(T[] items);
        public abstract void UpdateMany(T[] items, TID id);
        public async Task<T[]> GetAllAsync() => await Task.Run(GetAll).ConfigureAwait(false);
        public async Task<T> GetByIdAsync(TID id) => await Task.Run(() => GetById(id)).ConfigureAwait(false);
        public async Task<TID> InsertAsync(T item) => await Task.Run(() => Insert(item)).ConfigureAwait(false);
        public async Task DeleteItemAsync(T item) => await Task.Run(() => DeleteItem(item)).ConfigureAwait(false);
        public async Task DeleteByIdAsync(TID id) => await Task.Run(() => DeleteById(id)).ConfigureAwait(false);
        public async Task<TID> UpdateAsync(T item) => await Task.Run(() => Update(item)).ConfigureAwait(false);
        public async Task<TID> UpsertAsync(T item) => await Task.Run(() => Upsert(item)).ConfigureAwait(false);
        public async Task InsertManyAsync(T[] items) => await Task.Run(() => InsertMany(items)).ConfigureAwait(false);
        public async Task UpdateManyAsync(T[] items, TID id) => await Task.Run(() => UpdateMany(items, id)).ConfigureAwait(false);
    }
}
