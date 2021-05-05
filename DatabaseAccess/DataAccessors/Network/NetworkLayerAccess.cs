using System.Data;
using DataAccessLibrary.DataTransferObjects.NetworkDTOs;
using DataAccessLibrary.Internal.SQL.Enums;

namespace DataAccessLibrary.DataAccessors.Network
{
    public class NetworkLayerAccess : SqlDatabaseAccessAbstract<NetworkLayerDto>
    {
        protected override Table Table => Table.NETWORK_LAYER;
        protected override NetworkLayerDto CreateFromRow(DataRow row)
        {
            return new(row);
        }
    }
}