using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace UserControls.WpfOverrides.Converters
{
    public class NeuralnetDisplayLineThicknessConvertor : MarkupExtension, IValueConverter
    {
        public double Limit { get; set; }
        public bool Enabled { get; set; }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!Enabled)
                return 1;

            if (value is double d)
            {
                if (d == 0)
                    return .1d;

                if (d > 0)
                {
                    return (d < Limit ? d : Limit) / 3d;
                }

                return (d < -Limit ? -Limit : d) / 3d;
            }

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}