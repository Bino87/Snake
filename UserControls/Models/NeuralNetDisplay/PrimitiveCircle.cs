using UserControls.Enums;

namespace UserControls.Models.NeuralNetDisplay
{
    public class PrimitiveCircle : PrimitiveShape
    {
        public override ShapeType ShapeType => ShapeType.Circle;

        private double _radius;

        public double Radius
        {
            get => _radius;
            set => SetField(ref _radius, value);
        }


        public PrimitiveCircle(PrimitiveShapeValueProvider valueProvider, double x, double y, double radius) : base(valueProvider, x, y)
        {
            _radius = radius;
        }
    }
}