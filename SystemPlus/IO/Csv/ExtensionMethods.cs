using System;
using System.IO;

namespace SystemPlus.IO.Csv
{
    /// <summary>
    /// Extension methods to help writing csv data
    /// </summary>
    public static class ExtensionMethods
    {
        public static void WriteCsvVal(this TextWriter sw, string separator = ",")
        {
            sw.WriteCsvVal(null, separator);
        }

        public static void WriteCsvVal(this TextWriter sw, object? val, string separator = ",")
        {
            if (sw == null)
                throw new ArgumentNullException(nameof(sw));

            string sval = EscapeCsvField(val, separator);
            sw.Write(sval + separator);
        }

        public static void WriteCsvVals(this TextWriter sw, string separator, params object[] vals)
        {
            if (sw == null)
                throw new ArgumentNullException(nameof(sw));

            foreach (object o in vals)
            {
                sw.WriteCsvVal(o, separator);
            }

            sw.WriteLine();
        }

        public static string EscapeCsvField(object? value, string separator = ",")
        {
            if (value == null)
                return string.Empty;

            return EscapeCsvField(value.ToString(), separator);
        }

        public static string EscapeCsvField(string? value, string separator = ",")
        {
            if (string.IsNullOrEmpty(value))
                return string.Empty;

            value = value.Replace("\r\n", " ", StringComparison.InvariantCulture);
            value = value.Replace("\n", " ", StringComparison.InvariantCulture);
            value = value.Replace("\r", " ", StringComparison.InvariantCulture);

            if (value.Contains(separator, StringComparison.InvariantCulture))
            {
                if (value.Contains("\"", StringComparison.InvariantCulture))
                    value = "\"" + value.Replace("\"", "\"\"", StringComparison.InvariantCulture) + "\"";
                else
                    value = "\"" + value + "\"";
            }

            return value;
        }
    }
}