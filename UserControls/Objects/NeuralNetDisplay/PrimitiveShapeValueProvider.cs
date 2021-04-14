using UserControls.Core.Base;

namespace UserControls.Objects.NeuralNetDisplay
{
    public class PrimitiveShapeValueProvider : Observable
    {
        private double _value ;

        public double Value
        {
            get => _value;
            set => SetField(ref _value, value);
        }
    }
}