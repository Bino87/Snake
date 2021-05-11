using UserControls.Core.Base;

namespace UserControls.Models.SimulationRunner
{
    public class ProgressGraphLineViewModel : Observable
    {
        private double _x1, _x2, _y1, _y2;
        public double X1
        {
            get => _x1;
            set => SetField(ref _x1, value);
        }

        public double X2
        {
            get => _x2;
            set => SetField(ref _x2, value);
        }

        public double Y1
        {
            get => _y1;
            set => SetField(ref _y1, value);
        }

        public double Y2
        {
            get => _y2;
            set => SetField(ref _y2, value);
        }

        public ProgressGraphLineViewModel(double x1, double x2, double y1, double y2)
        {
            _x1 = x1;
            _x2 = x2;
            _y1 = y1;
            _y2 = y2;
        }
    }
}