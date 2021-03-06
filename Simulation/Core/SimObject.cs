using Simulation.Enums;

namespace Simulation.Core
{
    public abstract class SimObject
    {
        protected SimObject(int x, int y)
        {
            X = x;
            Y = y;
        }

        
        public abstract MapCellType Type { get; }
        public int X { get;  set; }
        public int Y { get; set; }
    }
}
