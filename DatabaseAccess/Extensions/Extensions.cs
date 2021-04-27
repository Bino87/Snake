using System;
using System.Data;
using DataAccessLibrary.DataTransferObjects;
using DataAccessLibrary.Internal.SQL;
using DataAccessLibrary.Internal.SQL.Enums;

namespace DataAccessLibrary.Extensions
{
    internal static class Extensions
    {
        internal static DataTable ToDataTable(this SqlDataTransferObject[] sqlDataTransferObjects)
        {
            DataTable dt = new DataTable();

            foreach (string columnName in sqlDataTransferObjects[0].ColumnNames())
            {
                dt.Columns.Add(columnName);
            }

            foreach (SqlDataTransferObject item in sqlDataTransferObjects)
            {
                DataRow row = dt.NewRow();

                item.FillDataRow(row);

                dt.Rows.Add(row);
            }

            return dt;
        }
    }
}
