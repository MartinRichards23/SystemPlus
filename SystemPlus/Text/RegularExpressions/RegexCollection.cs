using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace SystemPlus.Text.RegularExpressions
{
    /// <summary>
    /// A collection of Regex objects
    /// </summary>
    public class RegexCollection : List<Regex>
    {
        public void AddRange(IEnumerable<string> patterns, RegexOptions regexOptions)
        {
            foreach (string pattern in patterns)
            {
                Regex regex = new Regex(pattern, regexOptions);
                Add(regex);
            }
        }

        public void Add(string pattern)
        {
            Add(pattern, RegexOptions.None);
        }

        public void Add(string pattern, RegexOptions regexOptions)
        {
            Regex regex = new Regex(pattern, regexOptions);
            Add(regex);
        }

        /// <summary>
        /// Returns true if the input string matches any of the regexes in this collection
        /// </summary>
        public bool AnyMatch(string input)
        {
            foreach (Regex regex in this)
            {
                if (regex.IsMatch(input))
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Gets all the matches from this collection of regexes
        /// </summary>
        public IList<Match> AllMatches(string input)
        {
            IList<Match> matches = new List<Match>();

            foreach (Regex regex in this)
            {
                foreach (Match? m in regex.Matches(input))
                {
                    if (m != null)
                        matches.Add(m);
                }
            }

            return matches;
        }

        /// <summary>
        /// Counts the total number of matches with this collection of regexes
        /// </summary>
        public int CountMatches(string input)
        {
            int total = 0;

            foreach (Regex regex in this)
            {
                total += regex.Matches(input).Count;
            }

            return total;
        }
    }
}