using System.Data;
using DataAccessLibrary.DataTransferObjects.NetworkDTOs;
using DataAccessLibrary.Internal.SQL.Enums;

namespace DataAccessLibrary.DataAccessors.Network
{
    public class NetworkWeightAccess : SqlDatabaseAccessAbstract<NetworkWeightDto>
    {
        protected override Table Table => Table.NETWORK_WEIGHT;
        protected override NetworkWeightDto CreateFromRow(DataRow row)
        {
            return new(row);
        }
    }
}