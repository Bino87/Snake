using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;
using System.Windows.Media;
using Simulation.Enums;

namespace UserControls.Converters
{
    public class SnakeMapColorConverter : MarkupExtension, IValueConverter
    {
        public SolidColorBrush Snake{ get; set; }
        public SolidColorBrush Food { get; set; }
        public SolidColorBrush Head { get; set; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is MapCellType mcs)
            {
                return mcs switch
                    {
                        MapCellType.Snake => Snake,
                        MapCellType.Food => Food,
                        MapCellType.Head => Head,
                        _ => throw new ArgumentOutOfRangeException()
                    };
            }

            return null;
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
