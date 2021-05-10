using DataAccessLibrary.DataTransferObjects.NetworkDTOs;
using DataAccessLibrary.Internal.ParameterNames;
using DataAccessLibrary.Internal.SQL.Enums;
using MongoDB.Driver;

namespace DataAccessLibrary.DataAccessors.Network
{
    public class NetworkTemplateAccess : MongoDbDatabaseAccessAbstract<NetworkTemplateDto>
    {
        protected override Table Table => Table.NETWORK_TEMPLATE;
        public override FilterDefinition<NetworkTemplateDto> GetUpsertFilterDefinition(NetworkTemplateDto item)
        {
            return Builders<NetworkTemplateDto>.Filter.Eq(ParameterNames.MongoDb.cId, item.Id);
        }
    }
}