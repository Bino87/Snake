using System;
using System.Collections.Generic;
using System.Linq;

namespace Simulation.SimResults
{
    public readonly struct SimulationResult : IComparable
    {
        public int Generation { get; }
        public double Points { get; }


        public SimulationResult(int points, int generation, List<HashSet<int>> uniqueCells)
        {
            int steps = uniqueCells.Sum(t => t.Count);

            double a = points * 100; 
            Points = steps + a ;
            Generation = generation;

        }

        public int CompareTo(object? obj)
        {
            if (obj is SimulationResult sr)
            {
                //int genRes = Generation.CompareTo(sr.Generation);

                //if (genRes == 0)
                //{
                    int i = Points.CompareTo(sr.Points);
                    return i;
                //}

                //return genRes;
            }

            return 0;
        }
    }
}