using System;
using System.Collections.Generic;
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

        #region Static methods

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

        public static IEnumerable<WildCard> MakeWildcards(IEnumerable<string> patterns, RegexOptions options)
        {
            List<WildCard> regexes = new List<WildCard>();

            foreach (string pattern in patterns)
            {
                WildCard reg = new WildCard(pattern, options);
                regexes.Add(reg);
            }

            return regexes;
        }

        #endregion
    }
}