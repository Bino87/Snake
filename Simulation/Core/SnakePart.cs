using System;
using System.Diagnostics;
using Simulation.Enums;
using Simulation.Interfaces;

namespace Simulation.Core
{

    public class SnakeHead : SnakePart
    {
        public override MapCellType Type => MapCellType.Head;
        public SnakeHead(int x, int y, Direction direction) : base(x, y, direction)
        {
        }
    }
    [DebuggerDisplay("X1:{X} Y1:{Y} D:{Direction}")]
    public class SnakePart : SimObject
    {
        public override MapCellType Type => MapCellType.Snake;

        public Direction Direction { get; set; }
        public SnakePart(int x, int y, Direction direction) : base(x, y)
        {
            Direction = direction;
        }

        private (int X, int Y) GetMove(Direction direction)
        {
            return direction switch
            {

                Direction.Up => (0, -1),
                Direction.Right => (1, 0),
                Direction.Down => (0, 1),
                Direction.Left => (-1, 0),
                _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
            };
        }
        public (int X, int Y) GetPositionAfterMove(Direction direction)
        {
            (int x, int y) = GetMove(direction);
            return (X + x, Y + y);
        }

        public (int X, int Y) Move(Direction direction)
        {
            Direction = direction;
            (int x, int y) = GetPositionAfterMove(Direction);

          

            X = x;
            Y = y;
            return (X, Y);
        }
    }
}