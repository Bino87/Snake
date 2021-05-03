using System;

namespace DataAccessLibrary.Internal.MongoDB
{
    public record MongoDbCallParameter(string Name, object Value);

    public class MongoDbCallParameters
    {
        public int Count => _callParameters.Length;

        public MongoDbCallParameter this[int index] => _callParameters[index];

        private readonly MongoDbCallParameter[] _callParameters;
        private int _current = 0;
        internal MongoDbCallParameters(int count)
        {
            _callParameters = new MongoDbCallParameter[count];
        }

        public MongoDbCallParameters Add(string name, object value)
        {
            if (_current < _callParameters.Length)
            {
                _callParameters[_current] = new MongoDbCallParameter(name, value);
                _current++;
                return this;
            }

            throw new Exception("too many parameters");
        }
    }
}
