using System;
using System.Globalization;
using System.Windows.Data;

namespace SystemPlus.Windows.Converters
{
    /// <summary>
    /// Converts a Double to Int
    /// </summary>
    [ValueConversion(typeof(int), typeof(double))]
    public class DoubleIntConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return null;

            return System.Convert.ToDouble(value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return null;

            return System.Convert.ToInt32(value);
        }
    }
}