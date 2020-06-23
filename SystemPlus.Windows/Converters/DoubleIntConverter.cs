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
        public object? Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return 0;

            return System.Convert.ToDouble(value, CultureInfo.CurrentCulture);
        }

        public object? ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return 0;

            return System.Convert.ToInt32(value, CultureInfo.CurrentCulture);
        }
    }
}