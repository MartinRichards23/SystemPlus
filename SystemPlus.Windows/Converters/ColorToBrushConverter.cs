using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using SystemPlus.Windows.Media;

namespace SystemPlus.Windows.Converters
{
    /// <summary>
    /// Converts Color to a SolidColorBrush
    /// </summary>
    [ValueConversion(typeof(Color), typeof(SolidColorBrush))]
    public class ColorToBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return null;

            // For a more sophisticated converter, check also the targetType and react accordingly..
            if (value is Color)
            {
                Color color = (Color)value;
                return BrushCache.GetBrush(color);
            }

            // Could support more source types if wish

            Type type = value.GetType();
            throw new InvalidOperationException("Unsupported type [" + type.Name + "]");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}