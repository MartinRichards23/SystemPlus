using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace SystemPlus.Windows.Converters
{
    [ValueConversion(typeof(bool), typeof(Visibility))]
    public class BoolToVisConverter : IValueConverter
    {
        public Visibility TrueValue { get; set; } = Visibility.Visible;
        public Visibility FalseValue { get; set; } = Visibility.Collapsed;

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return FalseValue;

            bool b = (bool)value;

            if (b)
                return TrueValue;
            return FalseValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return false;

            Visibility v = (Visibility)value;

            if (v == TrueValue)
                return true;

            return false;
        }
    }
}