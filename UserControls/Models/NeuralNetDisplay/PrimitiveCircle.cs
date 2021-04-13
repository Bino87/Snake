using UserControls.Enums;

namespace UserControls.Models.NeuralNetDisplay
{
    public class PrimitiveCircle : PrimitiveShape
    {
        public override ShapeType ShapeType => ShapeType.Circle;

        private double _width, _height;

        public double Width
        {
            get => _width;
            set => SetField(ref _width, value);
        }

        public double Height
        {
            get => _height;
            set => SetField(ref _height, value);
        }

       
        public PrimitiveCircle(double x, double y, double width, double height) : base(x,y)
        {
            _width = width;
            _height = height;
        }
    }
}