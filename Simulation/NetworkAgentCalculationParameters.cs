using System.Collections.Generic;
using Simulation.Core;
using Simulation.Enums;

namespace Simulation
{
    internal class NetworkAgentCalculationParameters
    {
        public NetworkAgentCalculationParameters(SnakeHead head, SnakePart tail, Food food, Dictionary<(int, int), MapCellType> lookUp, double mapSize)
        {
            Head = head;
            Tail = tail;
            Food = food;
            LookUp = lookUp;
            MapSize = mapSize;
        }

        internal double MapSize { get; }

        internal SnakeHead Head { get; }
        internal SnakePart Tail { get; }
        internal Food Food { get; }
        internal Dictionary<(int, int), MapCellType> LookUp { get; }

    }
}