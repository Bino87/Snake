using System;
using System.Collections.Generic;
using System.Text;
using DataAccessLibrary.DataAccessors;
using DataAccessLibrary.DataTransferObjects;
using DataAccessLibrary.Internal.SQL;
using DataAccessLibrary.Internal.SQL.Enums;

namespace DataAccessLibrary.Helpers.SQL.HelperModules.Base
{
    internal record CreatorResult(string Data, string Name);
    internal abstract class SqlCreator<T> where T : SqlDataTransferObject
    {
        private readonly StringBuilder _sb;
        protected Table _table;
        protected SqlDatabaseAccessAbstract<T> _access;
        protected T _item;
        protected SqlCreator(SqlDatabaseAccessAbstract<T> access, T item, Table table)
        {
            _access = access;
            _item = item;
            _table = table;
            _sb = new StringBuilder();
        }

        internal CreatorResult Create()
        {
            CreateName();
            CreateBody();
            return Return();
        }

        protected void AppendLine()
        {
            _sb.AppendLine();
        }

        protected void AppendLine(string str)
        {
            _sb.AppendLine(str);
        }

        public override string ToString()
        {
            return _sb.ToString();
        }

        protected abstract void CreateName();

        protected abstract void CreateBody();

        protected abstract CreatorResult Return();

        protected string GetParametrized() => string.Join("," + Environment.NewLine, GetParameterValues());

        protected IEnumerable<string> GetParameterValues()
        {
            SqlCallParameters p = _access.CreateDefaultParameters(_item.ParametersCount, Actions.DELETE_BY_ID);
            SqlCallParameters parameters = _item.CreateParameters(p);

            for (int i = 0; i < _item.ParametersCount; i++)
            {
                SqlCallParameter parameter = parameters[i];

                yield return GetValue(parameter);
            }
        }

        protected abstract string GetValue(SqlCallParameter parameter);

        protected static string GetNameFromParameterType(DataType parameterDataType)
        {
            return parameterDataType switch
            {
                DataType.Int => "INT",
                DataType.String => "VARCHAR(MAX)",
                DataType.String25 => "VARCHAR(25)",
                DataType.String50 => "VARCHAR(50)",
                DataType.Double => "FLOAT",
                DataType.TimeStamp => "TIMESTAMP",
                DataType.DateTime => "DATETIME2",
                _ => throw new ArgumentOutOfRangeException(nameof(parameterDataType), parameterDataType, null)
            };
        }
    }
}
