using GammaGear.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace GammaGear.Converters
{
    [ValueConversion(typeof(int), typeof(InstallMode))]
    public class InstallModeToIndexConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (int)value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (ApplicationTheme)value;
        }
    }
}
