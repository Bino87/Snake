using System.Diagnostics;
using UserControls.Core.Base;
using Simulation.Enums;
using Simulation.Interfaces;

namespace UserControls.Models
{
    [DebuggerDisplay("X:{X} Y:{Y} {CellStatus}")]
    public class MapCell : Observable, IMapCell
    {
        private double _x;
        private double _y;
        private MapCellStatus _cellStatus;

        public MapCellStatus CellStatus
        {
            get => _cellStatus;
            set => SetField(ref _cellStatus, value);
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

        public MapCell(double x, double y, double width, double height)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
        }
    }
}