using Simulation.Enums;

namespace Simulation.Interfaces
{
    public interface IMapCell : IMapItem
    {
        MapCellStatus CellStatus { get; set; }
    }
}
