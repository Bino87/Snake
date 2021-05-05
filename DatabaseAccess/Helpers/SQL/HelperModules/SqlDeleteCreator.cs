﻿using DataAccessLibrary.DataAccessors;
using DataAccessLibrary.DataTransferObjects;
using DataAccessLibrary.Internal.ParameterNames;
using DataAccessLibrary.Internal.SQL.Enums;


namespace DataAccessLibrary.Helpers.SQL.HelperModules
{
    internal class SqlDeleteCreator<T> : SqlStoredProcedureCreator<T> where T : SqlDataTransferObject
    {
        public SqlDeleteCreator(SqlDatabaseAccessAbstract<T> access, T item, Table table) : base(access, item, table, Actions.DELETE_BY_ID)
        {
        }

        protected override void CreateBody()
        {
            _sb.AppendLine("AS");

            _sb.AppendLine($"\tDELETE FROM {_table} WHERE {ParameterNames.SQL.cId} = @{ParameterNames.SQL.cId}");
        }

        protected override void CreateParameters()
        {
            _sb.AppendLine($"\t@{ParameterNames.SQL.cId} INT");
        }
    }
}