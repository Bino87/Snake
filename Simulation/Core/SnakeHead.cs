using Simulation.Enums;

namespace Simulation.Core
{
    public class SnakeHead : SnakePart
    {
        public override MapCellType Type => MapCellType.Head;
        public SnakeHead(int x, int y, Direction direction) : base(x, y, direction)
        {
        }
    }
}