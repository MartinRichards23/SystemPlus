using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using SystemPlus.Collections.Generic;

namespace SystemPlus.Text
{
    /// <summary>
    /// Extension methods for strings
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// Limits the length of a string, adding "…" if trimmed
        /// </summary>
        public static string Clip(this string value, int max, string ending = "…")
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value));

            if (value.Length <= max)
                return value;

            return value.Substring(0, max) + ending;
        }

        /// <summary>
        /// Returns a value indicating whether the specified string occurs within this string, can be case insensitive
        /// </summary>
        public static bool Contains(this string value, string text, StringComparison comp)
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value));

            return value.IndexOf(text, comp) >= 0;
        }

        /// <summary>
        /// Trims the string, works on null values
        /// </summary>
        public static string? TryTrim(this string? value)
        {
            if (value == null)
                return null;

            return value.Trim();
        }

        /// <summary>
        /// Trims the string, works on null values
        /// </summary>
        public static string? TryTrim(this string? value, params char[] trimChars)
        {
            if (value == null)
                return null;

            return value.Trim(trimChars);
        }

        public static IEnumerable<string> Trim(this IEnumerable<string> values, params char[] trimChars)
        {
            List<string> output = new List<string>();

            if (values != null)
            {
                foreach (string s in values)
                {
                    string s2 = s.Trim(trimChars);
                    output.Add(s2);
                }
            }

            return output;
        }

        /// <summary>
        /// Concatenates the values using the specified separator
        /// </summary>
        public static string Join(this IEnumerable<string> values, string separator)
        {
            return string.Join(separator, values.Where(s => !string.IsNullOrWhiteSpace(s)));
        }

        /// <summary>
        /// Converts "joHn smitH" to "John Smith"
        /// </summary>
        public static string ToTitleCase(this string value)
        {
            return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(value);
        }

        /// <summary>
        /// Gets first x number of characters in string
        /// </summary>
        public static string GetBeginning(this string value, int count)
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value));

            if (value.Length > count)
                value = value.Substring(0, count);

            return value;
        }

        /// <summary>
        /// Gets last x number of characters in string
        /// </summary>
        public static string GetEnd(this string value, int count)
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value));

            if (value.Length > count)
                value = value.Remove(0, value.Length - count);

            return value;
        }

        /// <summary>
        /// Gets string between before and after strings, includes the tags
        /// </summary>
        public static string GetFragmentInclusive(this string value, string after, string before, StringComparison comparisonType)
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value));

            int start;
            int end;

            if (string.IsNullOrEmpty(after) || !value.Contains(after, comparisonType))
                start = 0;
            else
                start = value.IndexOf(after, comparisonType);

            if (string.IsNullOrEmpty(before) || !value.Contains(before, comparisonType))
                end = value.Length;
            else
                end = value.IndexOf(before, start, comparisonType) + after.Length + 1;

            return value[start..end];
        }

        /// <summary>
        /// Gets string between before and after strings
        /// </summary>
        public static string GetFragment(this string value, string? after, string? before, StringComparison comparisonType)
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value));

            int start;
            int end;

            if (string.IsNullOrEmpty(after) || !value.Contains(after, comparisonType))
                start = 0;
            else
                start = value.IndexOf(after, comparisonType) + after.Length;

            if (string.IsNullOrEmpty(before) || !value.Contains(before, comparisonType))
                end = value.Length;
            else
                end = value.IndexOf(before, start, comparisonType);

            return value[start..end];
        }

        /// <summary>
        /// Gets string between before and after strings
        /// </summary>
        public static string GetFragment(this string value, string? after, string? before)
        {
            return GetFragment(value, after, before, StringComparison.InvariantCulture);
        }

        /// <summary>
        /// Gets string after strings
        /// </summary>
        public static string GetFragment(this string value, string after)
        {
            return GetFragment(value, after, null, StringComparison.InvariantCulture);
        }

        /// <summary>
        /// Replaces the characters at the given location with the replacement text
        /// </summary>
        public static string ReplaceAt(this string value, string replacement, int start, int length)
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value));

            string replaced = value.Substring(0, start) + replacement + value.Substring(start + length);
            return replaced;
        }

        /// <summary>
        /// Makes the first character upper case
        /// </summary>
        public static string ToUpperFirst(this string value)
        {
            if (string.IsNullOrEmpty(value))
                return value;

            return char.ToUpperInvariant(value[0]) + value.Substring(1);
        }

        /// <summary>
        /// Makes the first character lower case
        /// </summary>
        public static string ToLowerFirst(this string value)
        {
            if (string.IsNullOrEmpty(value))
                return value;

            return char.ToLowerInvariant(value[0]) + value.Substring(1);
        }

        public static IEnumerable<string> SplitIntoChunks(this string value, int maxChunkSize)
        {
            for (int i = 0; i < value.Length; i += maxChunkSize)
                yield return value.Substring(i, Math.Min(maxChunkSize, value.Length - i));
        }

        /// <summary>
        /// Appends formatted string to end of stringbuilder
        /// </summary>
        public static StringBuilder AppendLine(this StringBuilder value, string format, params object[] args)
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value));

            string line = string.Format(CultureInfo.InvariantCulture, format, args);
            value.AppendLine(line);

            return value;
        }

        /// <summary>
        /// Determine if string is a valid 64-bit integer
        /// </summary>
        public static bool IsInt(this string value)
        {
            return long.TryParse(value, out _);
        }

        public static int ToInt(this string value, int defaultValue)
        {
            if (int.TryParse(value, out int val))
                return val;

            return defaultValue;
        }

        /// <summary>
        /// Removes pluralization from string
        /// </summary>
        public static string RemovePlural(this string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return value;

            if (value.EndsWith("ses", StringComparison.InvariantCultureIgnoreCase))
                return value.Remove(value.Length - 2);

            if (value.EndsWith("s", StringComparison.InvariantCultureIgnoreCase))
                return value.Remove(value.Length - 1);

            return value;
        }

        public static string ReplaceFirstOccurence(this string input, string search, string replacement)
        {
            if (input == null)
                throw new ArgumentNullException(nameof(input));
            if (search == null)
                throw new ArgumentNullException(nameof(search));

            int index = input.IndexOf(search, StringComparison.Ordinal);
            return index >= 0
                 ? input.Substring(0, index) + replacement + input.Substring(index + search.Length)
                 : input;
        }
    }
}