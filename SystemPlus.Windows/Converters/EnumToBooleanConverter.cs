using System;
using System.Globalization;
using System.Windows.Data;

namespace SystemPlus.Windows.Converters
{
    /// <summary>
    /// Determines if parameter is the given value
    /// </summary>
    public class EnumToBooleanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null && parameter == null)
                return true;

            if (value == null || parameter == null)
                return false;

            return value.Equals(parameter);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value == null)
                return Binding.DoNothing;

            return value.Equals(true) ? parameter : Binding.DoNothing;
        }
    }
}