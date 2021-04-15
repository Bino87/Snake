using Simulation.Enums;

namespace Simulation.Core
{
    public class Food : SimObject
    {
        public override MapCellType Type => MapCellType.Food;

        public Food(int x,int y) : base(x, y)
        {
        }
    }
}