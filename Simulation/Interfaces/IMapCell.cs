using Simulation.Enums;

namespace Simulation.Interfaces
{
    public interface IMapCell
    {
        MapCellStatus CellStatus { get; set; }
    }
}
