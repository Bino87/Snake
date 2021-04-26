using DatabaseAccess.Internal;

namespace DatabaseAccess.DataTransferObjects
{
    public abstract class DataTransferObject
    {
        internal abstract DatabaseType DbType { get; }
    }
}
