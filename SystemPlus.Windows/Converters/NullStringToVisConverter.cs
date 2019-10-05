using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace SystemPlus.Windows.Converters
{
    /// <summary>
    /// Returns Visibility.Collapsed if the String is null
    /// </summary>
    [ValueConversion(typeof(string), typeof(Visibility))]
    public class NullStringToVisConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string? s = value as string;

            if (string.IsNullOrEmpty(s))
                return Visibility.Collapsed;

            return Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}