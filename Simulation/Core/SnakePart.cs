using System;
using System.Diagnostics;
using Simulation.Enums;

namespace Simulation.Core
{
    [DebuggerDisplay("II:{X}")]
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
                Direction.North => (0, -1),
                Direction.East => (-1, 0),
                Direction.South => (0, 1),
                Direction.West => (1, 0),
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        public void Move()
        {
            var move = GetMove();
            X += move.X;
            Y += move.Y;
        }

        public bool IsValidMove(int numTiles)
        {
            (int x, int y) = GetMove();

            return X + x >= 0 && Y + y >= 0 && X + x < numTiles && Y + y <= numTiles;
        }
    }
}