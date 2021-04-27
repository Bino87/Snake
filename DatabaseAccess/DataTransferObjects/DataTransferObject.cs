using DataAccessLibrary.Internal.Enums;

namespace DataAccessLibrary.DataTransferObjects
{
    public abstract class DataTransferObject
    {
        internal abstract DatabaseType DbType { get; }
    }
}
