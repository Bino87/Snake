using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;
using System.Windows.Media;

namespace UserControls.Converters
{
    public class NeuralnetDisplayLineColorConvertor : MarkupExtension, IValueConverter
    {
        public SolidColorBrush Positive{ get; set; }
        public SolidColorBrush Negative{ get; set; }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {

            if (value is double d)
            {
                return d >= 0 ? Positive : Negative;
            }

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}