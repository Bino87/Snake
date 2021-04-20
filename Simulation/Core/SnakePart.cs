using System.Diagnostics;
using Simulation.Enums;
using Simulation.Extensions;

namespace Simulation.Core
{
    [DebuggerDisplay("X1:{X} Y1:{Y} T:{Type} D:{Direction}")]
    public class SnakePart : SimObject
    {
        public override MapCellType Type => MapCellType.Snake;

        public Direction Direction { get; set; }
        public SnakePart(int x, int y, Direction direction) : base(x, y)
        {
            Direction = direction;
        }

        public (int X, int Y) GetPositionAfterMove(Direction direction)
        {
            (int x, int y) = direction.GetMove();
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