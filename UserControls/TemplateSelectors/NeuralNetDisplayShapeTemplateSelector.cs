using System.Windows;
using System.Windows.Controls;
using UserControls.Constants;
using UserControls.Enums;
using UserControls.Models.NeuralNetDisplay;

namespace UserControls.TemplateSelectors
{
    public class NeuralNetDisplayShapeTemplateSelector : DataTemplateSelector
    {
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (container is FrameworkElement fe && item is PrimitiveShape shape)
            {
                string key = shape.ShapeType == ShapeType.Circle ? Cons.cShapeCircleDataTemplate : Cons.cShapeLineDataTemplate;
                return  fe.FindResource(key) as DataTemplate;
            }

            return base.SelectTemplate(item, container);
        }
    }
}
