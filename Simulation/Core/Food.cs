using Simulation.Enums;

namespace Simulation.Core
{
    public class Food : SimObject
    {
        public override MapCellStatus Status => MapCellStatus.Food;

        public Food(int x,int y) : base(x, y)
        {
        }
    }
}