using System.Collections.Generic;
using Simulation.Enums;

namespace Simulation
{
    public readonly struct SimulationResult
    {
        public SimulationResult(int points, List<int> moves)
        {
            Points = points;
            Moves = moves;
        }

        public int Points { get; }
        public List<int> Moves { get; }
    }
}