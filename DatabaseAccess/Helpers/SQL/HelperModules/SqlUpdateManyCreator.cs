﻿using DataAccessLibrary.DataAccessors;
using DataAccessLibrary.DataTransferObjects;
using DataAccessLibrary.Extensions;
using DataAccessLibrary.Internal.ParameterNames;
using DataAccessLibrary.Internal.SQL.Enums;


namespace DataAccessLibrary.Helpers.SQL.HelperModules
{
    internal class SqlUpdateManyCreator<T> : SqlStoredProcedureCreator<T> where T : SqlDataTransferObject
    {
        public SqlUpdateManyCreator(SqlDatabaseAccessAbstract<T> access, T item, Table table) : base(access, item, table, Actions.UPDATE_MANY)
        {
        }

        protected override void CreateBody()
        {
            _sb.AppendLine("AS");
            _sb.AppendLine("BEGIN");
            _sb.AppendLine($"\tMERGE [dbo].{_table} as dbTable");
            _sb.AppendLine($"\tUSING @{ParameterNames.SQL.cDataTable} as tbl");
            _sb.AppendLine("\tON (dbTable.Id = tbl.Id)");
            _sb.AppendLine();
            this.WhenMatched(_sb, _access, _item);

            _sb.AppendLine(";");
            _sb.AppendLine("END");
            _sb.AppendLine("RETURN 0");
        }

        protected override void CreateParameters()
        {
            _sb.AppendLine($"\t@{ParameterNames.SQL.cDataTable} [dbo].{_table}_TYPE READONLY");
        }
    }
}