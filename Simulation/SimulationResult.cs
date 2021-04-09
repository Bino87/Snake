using System.Collections.Generic;

namespace Simulation
{
    public readonly struct SimulationResult
    {
        public SimulationResult(int points, List<int> moves, bool selfCollision, bool outOfBounds)
        {
            Points = points;
            Moves = moves;
        }

        public int Points { get; }
        public List<int> Moves { get; }
    }
}