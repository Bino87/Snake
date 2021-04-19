using System;
using System.Diagnostics;

namespace Simulation.SimResults
{
    [DebuggerDisplay("{AgentID} : {_agentResults.Points}")]
    public class FitnessResults : IComparable
    {
        public int AgentID { get; }

        public int AgentIndex { get; }

        public SimulationResult Result => _agentResults;

        private  SimulationResult _agentResults { get; }
        public FitnessResults(int agentIndex, SimulationResult calculateFitness, int agentId)
        {
            AgentIndex = agentIndex;
            _agentResults = calculateFitness;
            AgentID = agentId;
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
