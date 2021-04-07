using UserControls.Core.Base;
using UserControls.Enums;

namespace UserControls.Models
{
    public class MapCell : Observable
    {
        private double x;
        private double y;
        private MapCellStatus mapCellStatus;

        public MapCellStatus MapCellStatus
        {
            get => mapCellStatus;
            set => SetField(ref mapCellStatus, value);
        }

        public double Width { get; }
        public double Height{ get; }

        public double X
        {
            get => x;
            set => SetField(ref x, value);
        }

        public double Y
        {
            get => y;
            set => SetField(ref y, value);
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