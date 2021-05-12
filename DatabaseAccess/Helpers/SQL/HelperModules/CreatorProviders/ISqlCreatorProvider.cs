using DataAccessLibrary.DataTransferObjects;
using DataAccessLibrary.Helpers.SQL.HelperModules.Base;

namespace DataAccessLibrary.Helpers.SQL.HelperModules.CreatorProviders  
{
    internal interface ISqlCreatorProvider<T> where T : SqlDataTransferObject
    {
        SqlCreator<T> Delete();
        SqlCreator<T> GetAll();
        SqlCreator<T> GetById();
        SqlCreator<T> Insert();
        SqlCreator<T> InsertMany();
        SqlCreator<T> Type();
        SqlCreator<T> Update();
        SqlCreator<T> UpdateMany();
        SqlCreator<T> Upsert();

    }
}