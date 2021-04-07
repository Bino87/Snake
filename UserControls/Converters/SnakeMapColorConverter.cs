using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;
using System.Windows.Media;
using UserControls.Enums;

namespace UserControls.Converters
{
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
