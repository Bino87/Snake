using System.Diagnostics;
using UserControls.Core.Base;
using Simulation.Enums;
using Simulation.Interfaces;

namespace UserControls.Objects.SnakeDisplay
{
    [DebuggerDisplay("X1:{X} Y1:{Y} {CellType}")]
    public class MapCell : Observable, IMapCell
    {
        private double _x;
        private double _y;
        private MapCellType _cellType;


        public MapItemType ItemType => MapItemType.Cell;
        public MapCellType CellType
        {
            get => _cellType;
            set => SetField(ref _cellType, value);
        }

        public double Width { get; }
        public double Height{ get; }
            
        public double X
        {
            get => _x;
            set => SetField(ref _x, value);
        }

        public double Y
        {
            get => _y;
            set => SetField(ref _y, value);
        }

        public MapCell(double x, double y, double width, double height, MapCellType cellType)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
            CellType = cellType;
        }
    }
}