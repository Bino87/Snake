using System;
using System.Collections.Generic;
using System.Text;
using DataAccessLibrary.DataTransferObjects;
using DataAccessLibrary.Internal;
using DataAccessLibrary.Internal.SQL;
using DataAccessLibrary.Internal.SQL.Enums;

namespace DataAccessLibrary.Helpers.HelperModules
{
    internal record CreatorResult(string Data, string Name);
    internal abstract class SqlCreator<T> where T : SqlDataTransferObject
    {
        protected readonly StringBuilder sb;
        protected Table table;
        protected SqlDatabaseAccessAbstract<T> access;
        protected T item;
        protected SqlCreator(SqlDatabaseAccessAbstract<T> access, T item, Table table)
        {
            this.access = access;
            this.item = item;
            this.table = table;
            sb = new StringBuilder();
        }

        internal CreatorResult Create()
        {
            CreateName();
            CreateBody();
            return Return();
        }

        protected abstract void CreateName();

        protected abstract void CreateBody();

        protected abstract CreatorResult Return();

        protected string GetParametrized(bool isType) => string.Join("," + Environment.NewLine, GetValues(isType));

        protected IEnumerable<string> GetValues(bool isType)
        {
            SqlCallParameters p = access.CreateDefaultParameters(item.ParametersCount, Actions.DELETE_BY_ID);
            SqlCallParameters parameters = item.CreateParameters(p);

            for (int i = 0; i < item.ParametersCount; i++)
            {
                SqlCallParameter parameter = parameters[i];

                if (isType)
                    yield return "\t" + string.Join(" ", parameter.ParameterName,
                        GetNameFromParameterType(parameter.DataType));
                else
                    yield return "\t" + string.Join(" ", "@" + parameter.ParameterName,
                        GetNameFromParameterType(parameter.DataType),
                        parameter.Direction == Direction.Output || parameter.Direction == Direction.InputOutput
                            ? "OUTPUT"
                            : "");


            }
        }

        string GetNameFromParameterType(DataType parameterDataType)
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
