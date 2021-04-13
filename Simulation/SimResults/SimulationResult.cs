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
            return steps;
            double d = (SelfCollision ? -1d : 0d);
            double a = (Math.Pow(2, Points) + Math.Pow(Points, 2.1) * 500);
            double b = Math.Pow(Points, 1.2) * Math.Pow(steps * .25, 1.3);
            return steps + a - b + d;


            //double avg = Moves.Average() * fitnessParameters.AvgFactor;
            //double min = Moves.Min()* fitnessParameters.MinFactor;
            //double max = Moves.Max() * fitnessParameters.MaxFactor;



            //double median = Moves[Moves.Count / 2] * fitnessParameters.MedianFactor;
            //double outOfBounds = OutOfBounds ? fitnessParameters.OutOfBounds : 0;
            //double selfCollisin = SelfCollision ? fitnessParameters.SelfCollision : 0;
            //double points = Points * fitnessParameters.PointsFactor;

            //return avg + min + max + median + outOfBounds + selfCollisin + points;
        }
    }
}