using System;
using System.Data.SqlClient;
using DataAccessLibrary.Internal.SQL.Enums;

namespace DataAccessLibrary.Internal.SQL
{
    internal record SqlCallParameter(string ParameterName, object Value, DataType DataType, Direction Direction)
    {
        internal SqlParameter ToSqlParameter() => new()
        {
            Direction = Direction.ToParameterDirection(),
            ParameterName = ParameterName,
            Value = Value,
            SqlDbType = DataType.ToSqlType()
        };
    }

    internal class SqlCallParameters
    {
        private readonly string _sqlStoredProcedureName;
        private readonly SqlCallParameter[] _sqlCallParameters;
        internal int ParameterCount => _sqlCallParameters.Length;
        private int _currentParameterIndex;

        public SqlCallParameter this[int index] => _sqlCallParameters[index];

        internal string StoredProcedure => _sqlStoredProcedureName.ToString();

        internal SqlCallParameters(int parametersCount, string sqlStoredProcedureName, Actions action) : this(parametersCount, sqlStoredProcedureName)
        {
            AddParameter(ParameterNames.ParameterNames.cAction, action, DataType.Int, Direction.Input);
        }

        private SqlCallParameters(int parametersCount, string sqlStoredProcedureName)
        {
            _sqlStoredProcedureName = sqlStoredProcedureName;
            _sqlCallParameters = new SqlCallParameter[parametersCount];
        }

        internal void AddParameter(string parameterName, object value, DataType dataType, Direction direction)
        {
            if (_currentParameterIndex >= _sqlCallParameters.Length)
            {
                throw new Exception("Parameters are full, double check what you're doing.");
            }

            _sqlCallParameters[_currentParameterIndex] =
                new SqlCallParameter(parameterName, value, dataType, direction);

            _currentParameterIndex++;
        }

        internal bool Validate()
        {
            if (_currentParameterIndex == _sqlCallParameters.Length)
                return true;
            return false;
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
