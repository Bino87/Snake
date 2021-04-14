using System;
using System.Diagnostics;
using Simulation.Enums;
using Simulation.Interfaces;

namespace Simulation.Core
{
    [DebuggerDisplay("X:{X} Y:{Y} D:{Direction}")]
    public class SnakePart : SimObject
    {
        public override MapCellStatus Status => MapCellStatus.Snake;

        public Direction Direction { get; set; }
        public SnakePart(int x, int y, Direction direction) : base(x, y)
        {
            Direction = direction;
        }

        public (int X, int Y) GetMove()
        {
            return Direction switch
            {
                Direction.Up => (0, -1),
                Direction.Right => (-1, 0),
                Direction.Down => (0, 1),
                Direction.Left => (1, 0),
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        public void Move()
        {
            (int x, int y) = GetMove();
            X += x;
            Y += y;
        }

        public bool IsValidMove(IMapCell[,] map)
        {
            (int x, int y) = GetMove();

            return X + x >= 0 && X + x < map.GetLength(0) &&
                   Y + y >= 0 && Y + y < map.GetLength(1);

        }
    }
}