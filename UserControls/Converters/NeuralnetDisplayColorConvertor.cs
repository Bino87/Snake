using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;
using System.Windows.Media;

namespace UserControls.Converters
{
    public class NeuralnetDisplayLineThicknessConvertor : MarkupExtension, IValueConverter
    {
        public double Limit { get; set; }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double d)
            {
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
    public class NeuralnetDisplayColorConvertor : MarkupExtension, IValueConverter
    {
        public SolidColorBrush Positive { get; set; }
        public SolidColorBrush Negative { get; set; }

        private LinearGradientBrush Gradient { get; set; }


        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }

        byte LinearInterp(byte start, byte end, double percentage) => (byte)(start + (byte)Math.Round(percentage * (end - start)));
        Color ColorInterp(Color start, Color end, double percentage)
        {
            byte r = LinearInterp(start.R, end.R, percentage);
            byte g = LinearInterp(start.G, end.G, percentage);
            byte b = LinearInterp(start.B, end.B, percentage);

            return Color.FromRgb(r, g, b);
        }

        Color GradientPick(double percentage, Color Start, Color Center, Color End)
        {
            if (percentage < 0.5)
                return ColorInterp(Start, Center, percentage / 0.5);
            else if (percentage == 0.5)
                return Center;
            else
                return ColorInterp(Center, End, (percentage - 0.5) / 0.5);
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {



            if (value is double d)
            {
                d = d < -1 ? -1 : d;
                d = d > 1 ? 1 : d;

                if (d == 0)
                    return new SolidColorBrush(Colors.White);
                if (d > 0)
                    return new SolidColorBrush(ColorInterp(Colors.White, Positive.Color, d));
                Color c = ColorInterp(Colors.White, Negative.Color, -d);

                return new SolidColorBrush(c);
            }

            return Positive;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}