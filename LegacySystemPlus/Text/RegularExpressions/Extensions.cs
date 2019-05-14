using System.Text.RegularExpressions;

namespace SystemPlus.Text.RegularExpressions
{
    public static class Extensions
    {
        /// <summary>
        /// Replace all matches with the given char
        /// </summary>
        public static string BlankOut(this Regex regex, string input, char blankChar = ' ')
        {
            MatchEvaluator mv = m => new string(blankChar, m.Length);
            return regex.Replace(input, mv);
        }
    }
}