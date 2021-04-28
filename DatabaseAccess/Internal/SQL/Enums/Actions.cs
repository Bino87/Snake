namespace DataAccessLibrary.Internal.SQL.Enums
{
    internal enum Actions
    {
        GET_ALL = 1,
        GET_BY_ID = 2,
        INSERT = 3,
        DELETE_BY_ID = 4,
        DELETE_ITEM = 5,
        UPDATE = 6,
        UPSERT = 7,
        INSERT_MANY = 8,
        UPDATE_MANY = 9,
        UPSERT_MANY = 10,
    }
}