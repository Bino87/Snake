using System;
using System.Collections.Generic;
using System.Linq;

namespace Simulation.SimResults
{
    public readonly struct SimulationResult
    {
        public bool OutOfBounds { get; }
        public bool SelfCollision { get; }
        public int Points { get; }
        public List<int> Moves { get; }

        public SimulationResult(int points, List<int> moves, bool selfCollision, bool outOfBounds)
        {
            Points = points;
            Moves = moves;
            Moves.Sort();
            SelfCollision = selfCollision;
            OutOfBounds = outOfBounds;
        }

        public double CalculateFitness(FitnessParameters fitnessParameters)
        {
            double steps = Moves.Sum();
            double a = (Math.Pow(2, Points) + Math.Pow(Points, 2.1) * 500);
            double b = Math.Pow(Points, 1.2) * Math.Pow(steps  , 1.3) * .25;
            return steps + a - b;
        }
    }
}