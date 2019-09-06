using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using SystemPlus.Text.RegularExpressions;

namespace SystemPlus.Text
{
    public static class StringTools
    {
        /// <summary>
        ///  returns the longest string
        /// </summary>
        public static string Longest(string stringA, string stringB)
        {
            if (stringA == null)
                return stringB;

            if (stringB == null)
                return stringA;

            if (stringA.Length > stringB.Length)
                return stringA;

            return stringB;
        }

        /// <summary>
        /// Trims whitespace from the end og all lines in string
        /// </summary>
        public static string TrimLineEnds(string value)
        {
            if (value == null)
                return null;

            string[] lines = value.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
            string newvalue = string.Join(Environment.NewLine, lines.Select(s => s.TrimEnd()));
            return newvalue;
        }

        /// <summary>
        /// Normalise new lines
        /// </summary>
        public static string NormaliseNewLines(string input)
        {
            if (input == null)
                return null;

            return CommonRegexes.NewLine.Replace(input, Environment.NewLine);
        }

        /// <summary>
        /// Collapses consecutive whitespace into a single space, newlines become a single newline
        /// </summary>
        public static string CollapseWhiteSpace(string input)
        {
            if (input == null)
                return null;

            Regex whiteSpace = new Regex(@"[ \t]+");
            Regex newlines = new Regex(@"[\r\n]+");

            input = whiteSpace.Replace(input, " ");
            input = newlines.Replace(input, "\r\n");
            return input;
        }

        /// <summary>
        /// Collapses all consecutive whitespace into a single space
        /// </summary>
        public static string CollapseAllWhiteSpace(string input, string replacement = " ")
        {
            if (input == null)
                return null;

            Regex whiteSpaceAll = new Regex(@"[\s]+");

            input = whiteSpaceAll.Replace(input, replacement);
            return input;
        }

        /// <summary>
        /// Splits a string into and array of words
        /// </summary>
        public static string[] SplitIntoWords(string input)
        {
            if (input == null)
                return new string[0];

            return CommonRegexes.WordSplit.Split(input);
        }

        public static List<string> SplitIntoLines(string text)
        {
            List<string> lines = new List<string>();

            if (string.IsNullOrEmpty(text))
                return lines;

            foreach (string line in text.Split('\n', '\r'))
            {
                if (string.IsNullOrWhiteSpace(line))
                    continue;

                lines.Add(line);
            }

            return lines;
        }

        /// <summary>
        /// removes all punctuation
        /// </summary>
        public static string RemovePunctuation(string input, string replacement = "")
        {
            if (input == null)
                return null;

            return CommonRegexes.PunctuationAll.Replace(input, replacement);
        }

        /// <summary>
        /// removes common punctuation
        /// </summary>
        public static string RemoveCommonPunctuation(string input, string replacement = "")
        {
            if (input == null)
                return null;

            return CommonRegexes.PunctuationCommon.Replace(input, replacement);
        }

        /// <summary>
        /// Removes all diacritics from the string, e.g. turns "öäü" into "oau"
        /// </summary>
        public static string RemoveDiacritics(string input)
        {
            string normalizedString = input.Normalize(NormalizationForm.FormD);
            StringBuilder stringBuilder = new StringBuilder();

            for (int i = 0; i < normalizedString.Length; i++)
            {
                char c = normalizedString[i];
                if (CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
                    stringBuilder.Append(c);
            }

            return stringBuilder.ToString();
        }

        /// <summary>
        /// Converts (01234) 123-456 to 01234123456
        /// </summary>
        public static string RemoveNonNumeric(string input)
        {
            if (input == null)
                return null;

            return CommonRegexes.NonNumeric.Replace(input, "");
        }

        /// <summary>
        /// Tests if string contains a-z chars
        /// </summary>
        public static bool ContainsCharacters(string input)
        {
            return CommonRegexes.Char.IsMatch(input);
        }

        /// <summary>
        /// Gets the shortest and longest of two strings
        /// </summary>
        public static void Swap(string a, string b, out string shortest, out string longest)
        {
            if (a.Length < b.Length)
            {
                shortest = a;
                longest = b;
            }
            else
            {
                shortest = b;
                longest = a;
            }
        }

        public static string PutInQuotes(string value)
        {
            if (value == null)
                return null;

            value = value.Trim('\"');
            return "\"" + value + "\"";
        }

        public static string JoinIgnoreNulls<T>(string separator, params T[] values)
        {
            if (values == null)
                return null;

            return string.Join(separator, values.Where(s => s != null));
        }

        public static string JoinIgnoreNulls<T>(string separator, IEnumerable<T> values)
        {
            if (values == null)
                return null;

            return string.Join(separator, values.Where(s => s != null));
        }

        /// <summary>
        /// Returns the human-readable bytes size
        /// </summary>
        /// <returns>E.g. 32 GB</returns>
        public static string FormatBytes(long byteCount, string format = "0.#")
        {
            // Get absolute value
            long absolute_i = (byteCount < 0 ? -byteCount : byteCount);
            // Determine the suffix and readable value
            string suffix;
            double readable;
            if (absolute_i >= 0x1000000000000000) // Exabyte
            {
                suffix = "EB";
                readable = (byteCount >> 50);
            }
            else if (absolute_i >= 0x4000000000000) // Petabyte
            {
                suffix = "PB";
                readable = (byteCount >> 40);
            }
            else if (absolute_i >= 0x10000000000) // Terabyte
            {
                suffix = "TB";
                readable = (byteCount >> 30);
            }
            else if (absolute_i >= 0x40000000) // Gigabyte
            {
                suffix = "GB";
                readable = (byteCount >> 20);
            }
            else if (absolute_i >= 0x100000) // Megabyte
            {
                suffix = "MB";
                readable = (byteCount >> 10);
            }
            else if (absolute_i >= 0x400) // Kilobyte
            {
                suffix = "KB";
                readable = byteCount;
            }
            else
            {
                return byteCount.ToString("0 B"); // Byte
            }

            // Divide by 1024 to get fractional value
            readable = (readable / 1024);

            // Return formatted number with suffix
            return string.Format("{0} {1}", readable.ToString(format), suffix);
        }
    }
}