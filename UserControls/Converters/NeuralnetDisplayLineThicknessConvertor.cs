﻿using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

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
}