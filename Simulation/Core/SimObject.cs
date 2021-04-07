using Simulation.Enums;

namespace Simulation.Core
{
    public abstract class SimObject
    {
        protected SimObject(int internalIndex)
        {
            InternalIndex = internalIndex;
        }

        
        public abstract MapCellStatus Status { get; }
        public int InternalIndex { get;  set; }
    }
}
