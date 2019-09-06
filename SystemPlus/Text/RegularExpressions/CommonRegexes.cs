using System.Text.RegularExpressions;

namespace SystemPlus.Text.RegularExpressions
{
    /// <summary>
    /// Common useful regexes
    /// </summary>
    public static class CommonRegexes
    {
        public static readonly Regex WordSplit = new Regex(@"[\s,:;@\|\(\)\<\>""]+", RegexOptions.Compiled);

        public static readonly Regex PunctuationAll = new Regex(@"\p{P}", RegexOptions.Compiled);
        public static readonly Regex PunctuationCommon = new Regex(@"[\(\),\._-]", RegexOptions.Compiled);

        public static readonly Regex NonNumeric = new Regex(@"[^\d]", RegexOptions.Compiled);
        public static readonly Regex Char = new Regex(@"[a-zA-Z]", RegexOptions.Compiled);

        /// <summary>
        /// Matches individual newlines of all types
        /// </summary>
        public static readonly Regex NewLine = new Regex(@"\r\n|\n\r|\n|\r", RegexOptions.Compiled);
    }
}