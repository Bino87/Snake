using UserControls.Core.Base;
using Simulation.Enums;

namespace UserControls.Models
{
    public class MapCell : Observable
    {
        private double _x;
        private double _y;
        private MapCellStatus _mapCellStatus;

        public MapCellStatus MapCellStatus
        {
            get => _mapCellStatus;
            set => SetField(ref _mapCellStatus, value);
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