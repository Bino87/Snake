using System;
using System.Globalization;
using System.Windows.Data;

namespace UserControls.Converters
{
    public class ValueTupleIntIntToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is ValueTuple<int, int> v)
            {
                return $"{v.Item1} / {v.Item2}";
            }

            throw new NotImplementedException();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}