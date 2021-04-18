using System;
using System.Collections.Generic;
using System.Linq;

namespace Simulation.SimResults
{
    public readonly struct SimulationResult : IComparable
    {
        public int Generation { get; }
        public double Points { get; }


        public SimulationResult(int points, int generation, List<HashSet<int>> uniqueCells, int maxMovesWithoutFood, double ratio)
        {
            int steps = uniqueCells[^1].Count;

            double a = points * maxMovesWithoutFood;
            Points = (steps + a ) * ratio ;
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