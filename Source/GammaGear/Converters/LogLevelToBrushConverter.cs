using GammaGear.Logging;
using GammaGear.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace GammaGear.Converters
{
    [ValueConversion(typeof(LogLevel), typeof(Brush))]
    public class LogLevelToBrushConverter : IValueConverter
    {
        private static readonly Dictionary<LogLevel, Brush> _brushes = new Dictionary<LogLevel, Brush>()
        {
            { LogLevel.Trace,       new SolidColorBrush(Color.FromRgb(   0, 129, 183)) },
            { LogLevel.Debug,       new SolidColorBrush(Color.FromRgb(   0, 183, 132)) },
            { LogLevel.Information, new SolidColorBrush(Color.FromRgb(  83, 183,   0)) },
            { LogLevel.Warning,     new SolidColorBrush(Color.FromRgb( 183, 153,   0)) },
            { LogLevel.Error,       new SolidColorBrush(Color.FromRgb( 183,  82,   0)) },
            { LogLevel.Critical,    new SolidColorBrush(Color.FromRgb( 183,  39,   0)) }
        };

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // If theme is dark?
            if (value is not LogLevel l)
            {
                throw new InvalidOperationException("value should always be a Log in this context");
            }
            return _brushes[l];
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new InvalidOperationException("Converting from SolidColorBrush to LogLevel is not implemented");
        }
    }
}
