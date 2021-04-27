using System;
using DataAccessLibrary.Internal.SQL.Enums;

namespace DataAccessLibrary.Internal.SQL
{
    internal class SqlCallParameter
    {
        private readonly string _parameterName;
        private readonly object _value;
        private readonly DataType _dataType;
        private readonly Direction _direction;

        public SqlCallParameter(string parameterName, object value, DataType dataType, Direction direction)
        {
            _parameterName = parameterName;
            _value = value;
            _dataType = dataType;
            _direction = direction;
        }
    }

    internal class SqlCallParameters
    {
        private readonly SqlCallParameter[] _sqlCallParameters;
        private int _currentParameterIndex = 0;

        internal SqlCallParameters(int parametersCount, Actions action) : this(parametersCount)
        {
            AddParameter(ParameterNames.ParameterNames.cAction, action, DataType.Int, Direction.Input);
        }

        internal SqlCallParameters(int parametersCount)
        {
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
    }
}
