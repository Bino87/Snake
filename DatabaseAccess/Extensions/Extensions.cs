using System;
using System.Data;
using DataAccessLibrary.DataTransferObjects;
using DataAccessLibrary.Internal.SQL;
using DataAccessLibrary.Internal.SQL.Enums;

namespace DataAccessLibrary.Extensions
{
    public static class Extensions
    {
        internal static DataTable ToDataTable(this SqlCallParameters[] para)
        {
            DataTable dt = new();

            for (int i = 0; i < para.Length; i++)
            {
                if (i == 0)
                {
                    for (int x = 0; x < para[i].ParameterCount; x++)
                    {
                        if (!dt.Columns.Contains(para[i][x].ParameterName))
                            dt.Columns.Add(para[i][x].ParameterName);
                    }
                }

                DataRow row = dt.NewRow();
                for (int x = 0; x < para[i].ParameterCount; x++)
                {
                    row[para[i][x].ParameterName] = para[i][x].Value;
                }

                dt.Rows.Add(row);
            }

            return dt;
        }
    }
}
