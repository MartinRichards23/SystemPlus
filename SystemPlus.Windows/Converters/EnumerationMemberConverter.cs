using System;
using System.Globalization;
using System.Windows.Data;
using SystemPlus.ComponentModel;

namespace SystemPlus.Windows.Converters
{
    /// <summary>
    /// Class for converting enum to description value in xaml
    /// </summary>
    public class EnumerationMemberConverter : IValueConverter
    {
        public object? Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                if (value == null)
                    return null;

                Enum e = (Enum)value;
                return e.ToDescriptionString();
            }
            catch (InvalidCastException ex)
            {
                throw new InvalidCastException($"Unable to cast value: '{value}' to enum", ex);
            }
        }

        public object? ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}