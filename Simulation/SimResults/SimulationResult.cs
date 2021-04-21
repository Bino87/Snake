using System;
using System.Collections.Generic;
using System.Linq;
using Simulation.Interfaces;

namespace Simulation.SimResults
{
    public readonly struct SimulationResult : IComparable
    {
        public int Generation { get; }
        public double Points { get; }



        public SimulationResult(int generation, List<HashSet<int>> uniqueCells, int maxMovesWithoutFood, double ratio)
        {
            int steps = uniqueCells[^1].Count;
            int points = uniqueCells.Count - 1;
            double a = points * maxMovesWithoutFood;
            Points = (steps + a) * ratio;
            Generation = generation;
        }

        private SimulationResult(double points, int generation)
        {
            Points = points;
            Generation = generation;
        }

        public int CompareTo(object? obj)
        {
            if (obj is SimulationResult sr)
            {
                int i = Points.CompareTo(sr.Points);
                return i;
            }

            return 0;
        }

        public static SimulationResult operator +(SimulationResult sr1, SimulationResult sr2) => new(sr1.Points + sr2.Points, sr1.Generation);
    }
}