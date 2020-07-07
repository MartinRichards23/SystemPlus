using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;

namespace SystemPlus.Text
{
    /// <summary>
    /// Replaces pattern with temp key, they key can be substituted with original value
    /// </summary>
    public class TextHider
    {
        readonly IList<KeyValuePair<string, string>> values = new List<KeyValuePair<string, string>>();
        readonly string format;
        int count;

        public TextHider(string format = "~~#({0})#~~")
        {
            this.format = format;
        }

        public int Count
        {
            get { return count; }
        }

        public string Hide(string text, string pattern, RegexOptions options)
        {
            return Hide(text, new Regex(pattern, options));
        }

        public string Hide(string text, Regex pattern)
        {
            if (pattern == null)
                throw new ArgumentNullException(nameof(pattern));

            MatchCollection matches = pattern.Matches(text);

            for (int i = matches.Count - 1; i >= 0; i--)
            {
                Match m = matches[i];

                count++;
                string key = string.Format(CultureInfo.InvariantCulture, format, count);

                values.Add(new KeyValuePair<string, string>(key, m.Value));

                text = text.ReplaceAt(key, m.Index, m.Length);
            }

            return text;
        }

        public string Show(string text)
        {
            if (text == null)
                throw new ArgumentNullException(nameof(text));

            // Do them in reverse so they are undone properly
            for (int i = values.Count - 1; i >= 0; i--)
            {
                KeyValuePair<string, string> kvp = values[i];
                text = text.Replace(kvp.Key, kvp.Value, StringComparison.InvariantCultureIgnoreCase);
            }

            return text;
        }
    }
}