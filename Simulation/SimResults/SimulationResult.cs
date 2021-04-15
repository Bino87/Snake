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

            double a = (Math.Pow(2, points) + Math.Pow(points, 2.1) * 100);
            double b = Math.Pow(points, 1.2) * Math.Pow(steps, 1.3) * .25;
            Points = steps + a - b;
            Generation = generation;

        }

        public int CompareTo(object? obj)
        {
            if (obj is SimulationResult sr)
            {
                int genRes = Generation.CompareTo(sr.Generation);

                if (genRes == 0)
                {
                    int i = Points.CompareTo(sr.Points);
                    return i;
                }

                return genRes;
            }

            return 0;
        }
    }
}