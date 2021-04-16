using Simulation.Enums;

namespace Simulation.Interfaces
{
    public interface IMapCell : IMapItem
    {
        MapCellType CellType { get; set; }
    }
}
