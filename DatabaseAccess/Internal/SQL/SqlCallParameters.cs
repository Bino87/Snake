using System;
using System.Data.SqlClient;
using DataAccessLibrary.Extensions;
using DataAccessLibrary.Internal.SQL.Enums;

namespace DataAccessLibrary.Internal.SQL
{
    internal class SqlCallParameters
    {
        private readonly SqlCallParameter[] _sqlCallParameters;
        private int _currentParameterIndex;

        public SqlCallParameter this[int index] => _sqlCallParameters[index];

        internal string StoredProcedure { get; }

        internal SqlCallParameters(int parametersCount, Table table, Actions action) : this(parametersCount)
        {
            StoredProcedure = table.CreateStoredProcedureName(action);
        }

        private SqlCallParameters(int parametersCount)
        {
            _sqlCallParameters = new SqlCallParameter[parametersCount];
        }

        internal void AddParameter(SqlCallParameter callParameter)
        {
            if (_currentParameterIndex >= _sqlCallParameters.Length)
            {
                throw new Exception("Parameters are full, double check what you're doing.");
            }

            _sqlCallParameters[_currentParameterIndex] = callParameter;

            _currentParameterIndex++;
        }
        internal void AddParameter(string parameterName, object value, DataType dataType, Direction direction)
        {
            AddParameter(new SqlCallParameter(parameterName, value, dataType, direction));
        }

        public void FillParameters(SqlParameterCollection cmdParameters)
        {
            for (int i = 0; i < _sqlCallParameters.Length; i++)
            {
                cmdParameters.Add(_sqlCallParameters[i].ToSqlParameter());
            }
        }
    }
}
