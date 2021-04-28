using System.Data;
using DataAccessLibrary.DataTransferObjects.NetworkDTOs;
using DataAccessLibrary.Internal;
using DataAccessLibrary.Internal.SQL.Enums;

namespace DataAccessLibrary.DataAccessors.Network
{
    public class NetworkBiasAccess : SqlDatabaseAccessAbstract<NetworkBiasDto>
    {
        protected override Table Table => Table.NETWORK_BIAS;
        protected override NetworkBiasDto CreateFromRow(DataRow row)
        {
            return new(row);
        }
    }
}