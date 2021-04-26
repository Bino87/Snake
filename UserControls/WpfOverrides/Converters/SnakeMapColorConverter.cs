using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;
using System.Windows.Media;
using Simulation.Enums;

namespace UserControls.WpfOverrides.Converters
{
    public class SnakeMapColorConverter : MarkupExtension, IValueConverter
    {
        public SolidColorBrush Snake { get; set; }
        public SolidColorBrush Food { get; set; }
        public SolidColorBrush Head { get; set; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value switch
            {
                MapCellType.Snake => Snake,
                MapCellType.Food => Food,
                MapCellType.Head => Head,
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
    }
}
