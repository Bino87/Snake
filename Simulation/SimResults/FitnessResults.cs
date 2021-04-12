using System;
using System.Diagnostics;

namespace Simulation.SimResults
{
    [DebuggerDisplay("{AgentID} : {_agentResults}")]
    public class FitnessResults : IComparable
    {
        private readonly int _agentIndex;
        private readonly int _agentId;

        public int AgentID => _agentId;
        public int AgentIndex => _agentIndex;
        public double Result => _agentResults;

        private  double _agentResults { get; }
        public FitnessResults(int agentIndex, double calculateFitness, int agentId)
        {
            _agentIndex = agentIndex;
            _agentResults = calculateFitness;
            _agentId = agentId;
        }

        public int CompareTo(object? obj)
        {
            if (obj is FitnessResults fr)
            {
                return fr._agentResults.CompareTo(_agentResults);
            }

            return 0;
        }
    }
}
