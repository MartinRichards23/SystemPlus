﻿using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace SystemPlus.Text.RegularExpressions
{
    public static class RegexHelper
    {
        /// <summary>
        /// Checks is a string is a valid regex
        /// </summary>
        public static bool VerifyRegex(string patten, out string error)
        {
            try
            {
                Regex regex = new Regex(patten);

                error = null;
                return true;
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return false;
            }
        }

        public static IEnumerable<Regex> MakeRegexes(IEnumerable<string> patterns, RegexOptions options)
        {
            List<Regex> regexes = new List<Regex>();

            foreach (string pattern in patterns)
            {
                Regex reg = new Regex(pattern, options);
                regexes.Add(reg);
            }

            return regexes;
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
    }
}