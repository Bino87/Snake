using UserControls.Enums;

namespace UserControls.Models.NeuralNetDisplay
{
    public class PrimitiveLine : PrimitiveShape
    {
        public override ShapeType ShapeType => ShapeType.Line;
        private double _x2, _y2;

        public double X2
        {
            get => _x2;
            set => SetField(ref _x2, value);
        }

        public double Y2
        {
            get => _y2;
            set => SetField(ref _y2, value);
        }

        public PrimitiveLine(PrimitiveShapeValueProvider valueProvider, double x, double x2, double y, double y2) : base(valueProvider,x, y)
        {
            _x2 = x2;
            _y2 = y2 ;
        }
    }
}