using System.Globalization;
using System.Windows.Data;

namespace SystemPlus.Windows.Converters
{
    /// <summary>
    /// Converts Bool to a Double
    /// </summary>
    [ValueConversion(typeof(bool), typeof(double))]
    public class BoolToDoubleConverter : IValueConverter
    {
        public double TrueValue { get; set; } = 1;
        public double FalseValue { get; set; } = 0.5;

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return TrueValue;

            bool b = (bool)value;
            return b ? TrueValue : FalseValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}