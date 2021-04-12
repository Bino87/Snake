using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;
using System.Windows.Media;
using Simulation.Enums;

namespace UserControls.Converters
{
    public class StringToIntConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {

            return value.ToString();

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value?.ToString() == "")
                return 1;
            return int.TryParse(value.ToString(), out int v) ? v : 1;
        }
    }
    public class SnakeMapColorConverter : MarkupExtension, IValueConverter
    {
        public SolidColorBrush Transparent { get; set; }
        public SolidColorBrush Snake{ get; set; }
        public SolidColorBrush Food { get; set; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is MapCellStatus mcs)
            {
                return mcs switch
                    {
                        MapCellStatus.Snake => Snake,
                        MapCellStatus.Food => Food,
                        MapCellStatus.Empty => Transparent,
                        _ => throw new ArgumentOutOfRangeException()
                    };
            }

            return Transparent;
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
