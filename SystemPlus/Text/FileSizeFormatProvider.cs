using System;
using System.Globalization;

namespace SystemPlus.Text
{
    public class FileSizeFormatProvider : IFormatProvider, ICustomFormatter
    {
        public object? GetFormat(Type? formatType)
        {
            if (formatType == typeof(ICustomFormatter)) 
                return this;

            return null;
        }

        const string fileSizeFormat = "fs";
        const decimal OneKiloByte = 1024M;
        const decimal OneMegaByte = OneKiloByte * 1024M;
        const decimal OneGigaByte = OneMegaByte * 1024M;

        public string Format(string? format, object? arg, IFormatProvider? formatProvider)
        {
            if (format == null || !format.StartsWith(fileSizeFormat, StringComparison.Ordinal))
            {
                return DefaultFormat(format, arg, formatProvider);
            }

            if (arg is string)
            {
                return DefaultFormat(format, arg, formatProvider);
            }

            decimal size;

            try
            {
                size = Convert.ToDecimal(arg, CultureInfo.InvariantCulture);
            }
            catch (InvalidCastException)
            {
                return DefaultFormat(format, arg, formatProvider);
            }

            string suffix;
            if (size > OneGigaByte)
            {
                size /= OneGigaByte;
                suffix = "GB";
            }
            else if (size > OneMegaByte)
            {
                size /= OneMegaByte;
                suffix = "MB";
            }
            else if (size > OneKiloByte)
            {
                size /= OneKiloByte;
                suffix = "kB";
            }
            else
            {
                suffix = " B";
            }

            string precision = format.Substring(2);
            if (string.IsNullOrEmpty(precision))
                precision = "2";

            string stringFormat = "{0:N" + precision + "}{1}";
            return string.Format(CultureInfo.InvariantCulture, stringFormat, size, suffix);
        }

        static string DefaultFormat(string? format, object? arg, IFormatProvider? formatProvider)
        {
            if (arg is IFormattable formattableArg)
            {
                return formattableArg.ToString(format, formatProvider);
            }

            return arg?.ToString() ?? string.Empty;
        }
    }
}