using System.Data;
using DataAccessLibrary.DataTransferObjects;

namespace DataAccessLibrary.Extensions
{
    internal static class Extensions
    {
        internal static DataTable ToDataTable(this SqlDataTransferObject[] dtos)
        {
            DataTable dt = new();

            foreach (string columnName in dtos[0].ColumnNames())
            {
                dt.Columns.Add(columnName);
            }

            foreach (SqlDataTransferObject dto in  dtos)
            {
                DataRow row = dt.NewRow();

                dto.FillDataRow(row);

                dt.Rows.Add(row);
            }

            return dt;
        }
    }
}
