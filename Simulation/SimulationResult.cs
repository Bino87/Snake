using System;
using System.Collections.Generic;

namespace Simulation
{
    public readonly struct SimulationResult
    {
        public SimulationResult(int points, List<int> moves, bool selfCollision, bool outOfBounds)
        {
            Points = points;
            Moves = moves;
            SelfCollision = selfCollision;
            OutOfMemory = outOfBounds;
        }

        public bool OutOfMemory { get; }
        public bool SelfCollision { get; }
        public int Points { get; }
        public List<int> Moves { get; }
    }
}