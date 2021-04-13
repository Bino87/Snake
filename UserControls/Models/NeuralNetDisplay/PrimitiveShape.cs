using UserControls.Core.Base;
using UserControls.Enums;

namespace UserControls.Models.NeuralNetDisplay
{
    public class PrimitiveShapeValueProvider : Observable
    {
        private double _val;

        public PrimitiveShapeValueProvider(double val)
        {
            _val = val;
        }

        public double Value => _val;

        public void Update()
        {
            OnPropertyChanged(nameof(Value));
        }
    }

    public abstract class PrimitiveShape : Observable
    {
        private double _x;
        private double _y;
        public abstract ShapeType ShapeType { get; }
        public PrimitiveShapeValueProvider ValueProvider { get; }

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

        protected PrimitiveShape(PrimitiveShapeValueProvider valueProviderProvider, double x, double y)
        {
            ValueProvider = valueProviderProvider;
            _x = x;
            _y = y;
        }
    }
}
