using System.Windows;
using System.Windows.Controls;
using UserControls.Constants;
using UserControls.Core.Enums;
using UserControls.Core.Objects.NeuralNetDisplay;


namespace UserControls.WpfOverrides.StyleSelectors
{
    public class NeuralNetShapeStyleSelector : StyleSelector
    {
        public override Style SelectStyle(object item, DependencyObject container)
        {
            if (container is FrameworkElement fe && item is PrimitiveShape shape)
            {
                string key = shape.ShapeType == ShapeType.Circle ? Cons.cShapeCircleDataTemplate : Cons.cShapeLineDataTemplate;
                return  fe.FindResource(key) as Style;
            }
            
            return base.SelectStyle(item, container);
        }
    }
}
