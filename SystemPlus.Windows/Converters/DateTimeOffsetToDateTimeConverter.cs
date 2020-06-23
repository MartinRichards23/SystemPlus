using System;
using System.Globalization;
using System.Windows.Data;

namespace SystemPlus.Windows.Converters
{
    /// <summary>
    /// Converts a DateTime to DateTimeOffset
    /// </summary>
    [ValueConversion(typeof(DateTimeOffset), typeof(DateTime))]
    public class DateTimeOffsetToDateTimeConverter : IValueConverter
    {
        public object? Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return null;

            DateTimeOffset dto = (DateTimeOffset)value;

            return dto.UtcDateTime;
        }

        public object? ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return null;

            DateTime dt = (DateTime)value;
            return new DateTimeOffset(dt);
        }
    }
}