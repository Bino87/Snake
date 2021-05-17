using DataAccessLibrary.DataAccessors;
using DataAccessLibrary.DataTransferObjects;
using DataAccessLibrary.Helpers.SQL.HelperModules.Base;
using DataAccessLibrary.Helpers.SQL.HelperModules.Base.NetworkWeightAndBias;
using DataAccessLibrary.Internal.SQL.Enums;

namespace DataAccessLibrary.Helpers.SQL.HelperModules.CreatorProviders
{
    internal class WeightAndBiasSqlCreatorProvider<T> : SqlCreatorProvider<T> where T : SqlDataTransferObject
    {
        public WeightAndBiasSqlCreatorProvider(Table table, SqlDatabaseAccessAbstract<T> access, T item) : base(table, access, item)
        {
        }

        public override SqlCreator<T> Insert()
        {
            return new WeightAndBiasSqlInsertCreator<T>(_access, _item, _table);
        }

        public override SqlCreator<T> GetAll()
        {
            return new WeightAndBiasSqlGetAllCreator<T>(_access, _item, _table);
        }

        public override SqlCreator<T> InsertMany()
        {
            return new WeightAndBiasSqlInsertMany<T>(_access, _item, _table);
        }

        public override SqlCreator<T> GetById()
        {
            return new WeightAndBiasSqlGetByIdCreator<T>(_access, _item, _table); 
        }

        public override SqlCreator<T> Update()
        {
            return new WeightAndBiasSqlUpdateCreator<T>(_access, _item, _table); 
        }

        public override SqlCreator<T> Upsert()
        {
            return new WeightAndBiasSqlUpsertCreator<T>(_access, _item, _table);
        }
    }
}