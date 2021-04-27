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

    }
}
