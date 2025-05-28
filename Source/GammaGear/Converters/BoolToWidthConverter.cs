using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows;

namespace GammaGear.Converters
{
    [ValueConversion(typeof(bool), typeof(GridLength))]
    public sealed class BoolToWidthConverter : IValueConverter
    {
        public GridLength TrueValue { get; set; }
        public GridLength FalseValue { get; set; }

        public BoolToWidthConverter()
        {
            // set defaults
            TrueValue = new GridLength(1, GridUnitType.Star);
            FalseValue = new GridLength(0, GridUnitType.Star);
        }

        public object Convert(object value, Type targetType,
            object parameter, CultureInfo culture)
        {
            if (!(value is bool))
                return null;
            return (bool)value ? TrueValue : FalseValue;
        }

        public object ConvertBack(object value, Type targetType,
            object parameter, CultureInfo culture)
        {
            if (Equals(value, TrueValue))
                return true;
            if (Equals(value, FalseValue))
                return false;
            return null;
        }
    }
}
