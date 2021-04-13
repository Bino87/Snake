using System;
using System.Globalization;
using System.Windows.Data;

namespace UserControls.Converters
{
    public class StringToIntConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (string.IsNullOrWhiteSpace(value?.ToString()))
                return 1;
            return int.TryParse(value.ToString(), out int v) ? v : 1;
        }
    }
}