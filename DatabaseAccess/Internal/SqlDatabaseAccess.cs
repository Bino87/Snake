using DataAccessLibrary.Core;
using DataAccessLibrary.DataTransferObjects;
using DataAccessLibrary.Internal.SQL;
using DataAccessLibrary.Internal.SQL.Enums;
using DataAccessLibrary.Internal.SQL.ParameterNames;

namespace DataAccessLibrary.Internal
{
    internal class SqlDatabaseAccessAbstract<T> : DatabaseAccessAbstract<T> where T : SqlDataTransferObject
    {
        public override T[] GetAll()
        {
            SqlCallParameters parameters = CreateDefaultParameters(1, Actions.SelectAll);
            throw new System.NotImplementedException();
        }

        public override T GetById(int id)
        {
            SqlCallParameters parameters = CreateDefaultParameters(2, Actions.SelectById, id);
            throw new System.NotImplementedException();
        }

        public override int Insert(T item)
        {
            SqlCallParameters parameters = item.CreateParameters(CreateDefaultParameters(item.ParametersCount, Actions.Insert));

            throw new System.NotImplementedException();
        }

        public override void DeleteItem(T item)
        {
            SqlCallParameters parameters = item.CreateParameters(CreateDefaultParameters(item.ParametersCount, Actions.DeleteItem));
            throw new System.NotImplementedException();
        }

        public override void DeleteById(int id)
        {
            SqlCallParameters parameters = CreateDefaultParameters(2, Actions.DeleteById, id);
            throw new System.NotImplementedException();
        }

        public override int Update(T item)
        {
            SqlCallParameters parameters = item.CreateParameters(CreateDefaultParameters(item.ParametersCount, Actions.Update));
            throw new System.NotImplementedException();
        }

        public override int Upsert(T item)
        {
            SqlCallParameters parameters = item.CreateParameters(CreateDefaultParameters(item.ParametersCount, Actions.Upsert));
            throw new System.NotImplementedException();
        }

        private static SqlCallParameters CreateDefaultParameters(int parametersCount, Actions action)
        {
            SqlCallParameters parameters = new(parametersCount, action);
            return parameters;
        }

        private static SqlCallParameters CreateDefaultParameters(int parametersCount, Actions action, int id)
        {
            SqlCallParameters parameters = CreateDefaultParameters(parametersCount, action);
            parameters.AddParameter(ParameterNames.cId, id, DataType.Int, Direction.InputOutput);
            return parameters;
        }
    }

    internal abstract class MongoDbDatabaseAccessAbstract<T> : DatabaseAccessAbstract<T> where T : MongoDbDataTransferObject
    {

    }
}
