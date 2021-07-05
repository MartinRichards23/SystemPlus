using System.Text.RegularExpressions;

namespace SystemPlus.Text.RegularExpressions
{
    /// <summary>
    /// Common useful regexes
    /// </summary>
    public static class CommonRegexes
    {
        public static Regex WordSplit { get; } = new Regex(@"[\s,:;@\|\(\)\<\>""]+", RegexOptions.Compiled);

        /// <summary>
        /// Matches all punctuation
        /// </summary>
        public static Regex PunctuationAll { get; } = new Regex(@"\p{P}", RegexOptions.Compiled);

        /// <summary>
        /// Matches characters (),._-
        /// </summary>
        public static Regex PunctuationCommon { get; } = new Regex(@"[\(\),\._-]", RegexOptions.Compiled);

        /// <summary>
        /// Matches non numeric characters
        /// </summary>
        public static Regex NonNumeric { get; } = new Regex(@"[^\d\.,]", RegexOptions.Compiled);

        /// <summary>
        /// Matches a-z upper and lower case
        /// </summary>
        public static Regex Char { get; } = new Regex(@"[a-zA-Z]", RegexOptions.Compiled);

        /// <summary>
        /// Matches individual newlines of all types
        /// </summary>
        public static Regex NewLine { get; } = new Regex(@"\r\n|\n\r|\n|\r", RegexOptions.Compiled);
    }
}