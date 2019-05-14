using System;
using System.Text.RegularExpressions;

namespace SystemPlus.Text.RegularExpressions
{
    /// <summary>
    /// Represents a wildcard
    /// </summary>
    [Serializable]
    public class WildCard : Regex
    {
        /// <summary>
        /// Initializes a wildcard with the given search pattern.
        /// </summary>
        public WildCard(string pattern)
            : base(WildcardToRegex(pattern))
        {
        }

        /// <summary>
        /// Initializes a wildcard with the given search pattern and options.
        /// </summary>
        public WildCard(string pattern, RegexOptions options)
            : base(WildcardToRegex(pattern), options)
        {
        }

        /// <summary>
        /// Converts a wildcard to a regex.
        /// </summary>
        public static string WildcardToRegex(string pattern)
        {
            string regex = Escape(pattern);
            regex = regex.Replace("\\*", ".*");
            regex = regex.Replace("\\?", ".");
            regex = regex.Replace("\\|\\|", "|");

            return regex;
        }
    }
}