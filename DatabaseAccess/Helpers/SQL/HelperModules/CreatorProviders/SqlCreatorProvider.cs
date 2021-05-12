using DataAccessLibrary.DataAccessors;
using DataAccessLibrary.DataTransferObjects;
using DataAccessLibrary.Helpers.SQL.HelperModules.Base;
using DataAccessLibrary.Internal.SQL.Enums;

namespace DataAccessLibrary.Helpers.SQL.HelperModules.CreatorProviders
{
    internal class SqlCreatorProvider<T> : ISqlCreatorProvider<T> where T : SqlDataTransferObject
    {
        protected readonly Table _table;
        protected readonly SqlDatabaseAccessAbstract<T> _access;
        protected readonly T _item;

        public SqlCreatorProvider(Table table, SqlDatabaseAccessAbstract<T> access, T item)
        {
            _table = table;
            _access = access;
            _item = item;
        }

        public virtual SqlCreator<T> Delete()
        {
            return new DefaultSqlDeleteCreator<T>(_access, _item, _table);
        }

        public virtual SqlCreator<T> GetAll()
        {
            return new DefaultSqlGetAllCreator<T>(_access, _item, _table);
        }

        public virtual SqlCreator<T> GetById()
        {
            return new DefaultSqlGetByIdCreator<T>(_access, _item, _table);
        }

        public virtual SqlCreator<T> Insert()
        {
            return new DefaultSqlInsertCreator<T>(_access, _item, _table);
        }

        public virtual SqlCreator<T> InsertMany()
        {
            return new DefaultSqlInsertManyCreator<T>(_access, _item, _table);
        }

        public virtual SqlCreator<T> Type()
        {
            return new DefaultSqlTypeCreator<T>(_access, _item, _table);
        }

        public virtual SqlCreator<T> Update()
        {
            return new DefaultSqlUpdateCreator<T>(_access, _item, _table);
        }

        public virtual SqlCreator<T> UpdateMany()
        {
            return new DefaultSqlUpdateManyCreator<T>(_access, _item, _table);
        }

        public virtual SqlCreator<T> Upsert()
        {
            return new DefaultSqlUpsertCreator<T>(_access, _item, _table);
        }
    }
}