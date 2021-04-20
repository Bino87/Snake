using Simulation.Enums;

namespace Simulation.Core
{
    public class CellUpdateData
    {
        public CellUpdateData(int x, int y, MapCellType cellType)
        {
            X = x;
            Y = y;
            CellType = cellType;
        }

        public int X { get; }
        public int Y { get; }

        public MapCellType CellType { get; }
    }
}