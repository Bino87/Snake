using System;
using System.Diagnostics;

namespace Simulation.SimResults
{
    [DebuggerDisplay("ID:{AgentID} : Gen:{AgentResults.Generation} Points:{AgentResults.Points}")]
    public class FitnessResults : IComparable
    {
        public int AgentID { get; }

        public int AgentIndex { get; }

        public SimulationResult Result => AgentResults;

        private  SimulationResult AgentResults { get; }
        public FitnessResults(int agentIndex, SimulationResult calculateFitness, int agentId)
        {
            AgentIndex = agentIndex;
            AgentResults = calculateFitness;
            AgentID = agentId;
        }

        public int CompareTo(object obj)
        {
            if (obj is FitnessResults fr)
            {
                return fr.AgentResults.CompareTo(AgentResults);
            }

            return 0;
        }
    }
}
