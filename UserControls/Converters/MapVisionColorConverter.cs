using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;
using System.Windows.Media;
using Simulation.Enums;

namespace UserControls.Converters
{
    public class MapVisionColorConverter : MarkupExtension, IValueConverter
    {
        public SolidColorBrush Normal { get; set; }
        public SolidColorBrush Food { get; set; }
        public SolidColorBrush Self { get; set; }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value switch
            {
                VisionCollisionType.Normal => Normal,
                VisionCollisionType.Self => Self,
                VisionCollisionType.Food => Food,
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}