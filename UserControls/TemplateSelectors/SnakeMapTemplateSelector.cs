using System.Windows;
using System.Windows.Controls;
using Simulation.Enums;
using Simulation.Interfaces;
using UserControls.Constants;

namespace UserControls.TemplateSelectors
{
    public class SnakeMapTemplateSelector : DataTemplateSelector
    {
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (container is FrameworkElement fe && item is IMapItem shape)
            {
                string key = shape.ItemType == MapItemType.Cell ? Cons.cRectangleDataTemplate : Cons.cVisionDataTemplate;
                return  fe.FindResource(key) as DataTemplate;
            }

            return base.SelectTemplate(item, container);
        }
    }
}