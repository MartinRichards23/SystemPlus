using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using SystemPlus.Collections.Generic;

namespace SystemPlus.Text
{
    /// <summary>
    /// Various algorithms for comparing strings
    /// </summary>
    [SuppressMessage("Performance", "CA1814:Prefer jagged arrays over multidimensional")]
    public static class StringDifference
    {
        /// <summary>
        /// Compute Levenshtein distance
        /// </summary>
        /// <param name="s">String 1</param>
        /// <param name="t">String 2</param>
        /// <returns>The edit distance between the two strings.</returns>
        public static int Levenshtein(string s, string t)
        {
            if (s == null)
                throw new ArgumentNullException(nameof(s));
            if (t == null)
                throw new ArgumentNullException(nameof(t));

            int n = s.Length; //length of s
            int m = t.Length; //length of t

            int[,] d = new int[n + 1, m + 1]; // matrix

            // Step 1
            if (n == 0)
                return m;
            if (m == 0)
                return n;

            // Step 2
            for (int i = 0; i <= n; i++)
            {
                d[i, 0] = i;
            }

            for (int j = 0; j <= m; j++)
            {
                d[0, j] = j;
            }


            // Step 3
            for (int i = 1; i <= n; i++)
            {
                //Step 4
                for (int j = 1; j <= m; j++)
                {
                    // Step 5
                    int cost = (t.Substring(j - 1, 1) == s.Substring(i - 1, 1) ? 0 : 1); // cost

                    // Step 6                    
                    d[i, j] = Math.Min(Math.Min(d[i - 1, j] + 1, d[i, j - 1] + 1), d[i - 1, j - 1] + cost);
                }
            }

            // Step 7
            return d[n, m];
        }

        /// <summary>
        /// Compute Damerau-Levenshtein distance
        /// </summary>
        /// <param name="source"></param>
        /// <param name="target"></param>
        /// <returns>The edit distance between the two strings.</returns>
        public static int DamLevenshtein(string source, string target)
        {
            if (string.IsNullOrEmpty(source))
            {
                if (string.IsNullOrEmpty(target))
                    return 0;

                return target.Length;
            }

            if (string.IsNullOrEmpty(target))
                return source.Length;

            int m = source.Length;
            int n = target.Length;
            int[,] h = new int[m + 2, n + 2];

            int inf = m + n;
            h[0, 0] = inf;

            for (int i = 0; i <= m; i++)
            {
                h[i + 1, 1] = i;
                h[i + 1, 0] = inf;
            }

            for (int j = 0; j <= n; j++)
            {
                h[1, j + 1] = j;
                h[0, j + 1] = inf;
            }

            SortedDictionary<char, int> sd = new SortedDictionary<char, int>();
            foreach (char letter in (source + target))
            {
                if (!sd.ContainsKey(letter))
                    sd.Add(letter, 0);
            }

            for (int i = 1; i <= m; i++)
            {
                int db = 0;
                for (int j = 1; j <= n; j++)
                {
                    int i1 = sd[target[j - 1]];
                    int j1 = db;

                    if (source[i - 1] == target[j - 1])
                    {
                        h[i + 1, j + 1] = h[i, j];
                        db = j;
                    }
                    else
                    {
                        h[i + 1, j + 1] = Math.Min(h[i, j], Math.Min(h[i + 1, j], h[i, j + 1])) + 1;
                    }

                    h[i + 1, j + 1] = Math.Min(h[i + 1, j + 1], h[i1, j1] + (i - i1 - 1) + 1 + (j - j1 - 1));
                }

                sd[source[i - 1]] = i;
            }

            return h[m + 1, n + 1];
        }

        /// <summary>
        /// Calculates the edit distance between two arrays of strings
        /// </summary>
        public static int WordEditDistance(string[] source, string[] target)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (target == null)
                throw new ArgumentNullException(nameof(target));

            // deal with null or empty situations
            if (source.IsNullOrEmpty() || target.IsNullOrEmpty())
            {
                if (source.IsNullOrEmpty() && target.IsNullOrEmpty())
                    return 0;

                if (source.IsNullOrEmpty())
                    return target.Length;
                if (target.IsNullOrEmpty())
                    return source.Length;
            }

            int m = source.Length;
            int n = target.Length;
            int[,] h = new int[m + 2, n + 2];

            int inf = m + n;
            h[0, 0] = inf;

            for (int i = 0; i <= m; i++)
            {
                h[i + 1, 1] = i;
                h[i + 1, 0] = inf;
            }

            for (int j = 0; j <= n; j++)
            {
                h[1, j + 1] = j;
                h[0, j + 1] = inf;
            }

            SortedDictionary<string, int> sd = new SortedDictionary<string, int>();
            foreach (string letter in source)
            {
                if (!sd.ContainsKey(letter))
                    sd.Add(letter, 0);
            }
            foreach (string letter in target)
            {
                if (!sd.ContainsKey(letter))
                    sd.Add(letter, 0);
            }

            for (int i = 1; i <= m; i++)
            {
                int db = 0;
                for (int j = 1; j <= n; j++)
                {
                    int i1 = sd[target[j - 1]];
                    int j1 = db;

                    if (source[i - 1] == target[j - 1])
                    {
                        h[i + 1, j + 1] = h[i, j];
                        db = j;
                    }
                    else
                    {
                        h[i + 1, j + 1] = Math.Min(h[i, j], Math.Min(h[i + 1, j], h[i, j + 1])) + 1;
                    }

                    h[i + 1, j + 1] = Math.Min(h[i + 1, j + 1], h[i1, j1] + (i - i1 - 1) + 1 + (j - j1 - 1));
                }

                sd[source[i - 1]] = i;
            }

            return h[m + 1, n + 1];
        }

        public static double WordEditDistancePercent(string[] source, string[] target)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (target == null)
                throw new ArgumentNullException(nameof(target));

            double distance = WordEditDistance(source, target);
            double length = Math.Max(source.Length, target.Length);
            return 1 - (distance / length);
        }

        /// <summary>
        /// Compute Damerau-Levenshtein distance
        /// </summary>
        /// <returns>Normalised value, 0 being all letters were changes, 1 being they were exactly the same</returns>
        public static double DamLevenPercent(string source, string target)
        {
            double distance = DamLevenshtein(source, target);
            double length = Math.Max(source.Length, target.Length);
            return 1 - (distance / length);
        }

        /// <summary>
        /// Compute Jaro-Winkler similarity
        /// </summary>
        /// <param name="s1"></param>
        /// <param name="s2"></param>
        /// <returns>Returns normalized score, with 0.0 meaning no similarity at all, and 1.0 meaning full equality.</returns>        
        public static double JaroWinkler(string s1, string s2)
        {
            if (s1 == null)
                throw new ArgumentNullException(nameof(s1));
            if (s2 == null)
                throw new ArgumentNullException(nameof(s2));

            if (s1 == s2)
                return 1.0;

            // ensure that s1 is shorter than or same length as s2
            if (s1.Length > s2.Length)
            {
                // swap them
                string tmp = s2;
                s2 = s1;
                s1 = tmp;
            }

            // (1) find the number of characters the two strings have in common.
            // note that matching characters can only be half the length of the
            // longer string apart.
            int maxdist = s2.Length / 2;
            int c = 0; // count of common characters
            int t = 0; // count of transpositions
            int prevpos = -1;
            for (int ix = 0; ix < s1.Length; ix++)
            {
                char ch = s1[ix];

                // now try to find it in s2
                for (int ix2 = Math.Max(0, ix - maxdist); ix2 < Math.Min(s2.Length, ix + maxdist); ix2++)
                {
                    if (ch == s2[ix2])
                    {
                        c++; // we found a common character
                        if (prevpos != -1 && ix2 < prevpos)
                            t++; // moved back before earlier 
                        prevpos = ix2;
                        break;
                    }
                }
            }

            // we might have to give up right here
            if (c == 0)
                return 0.0;

            // first compute the score
            double score = ((c / (double)s1.Length) + (c / (double)s2.Length) + ((c - t) / (double)c)) / 3.0;

            // (2) common prefix modification
            int p = 0; // length of prefix
            int last = Math.Min(4, s1.Length);
            for (; p < last && s1[p] == s2[p]; p++)
            {
                score += ((p * (1 - score)) / 10);
            }

            return score;
        }

        public static double ContainsWords(string a, string b)
        {
            if (a == null)
                throw new ArgumentNullException(nameof(a));
            if (b == null)
                throw new ArgumentNullException(nameof(b));

            string[] aWords = StringTools.SplitIntoWords(a);
            double score = 0;

            for (int i = 0; i < aWords.Length; i++)
            {
                string word = aWords[i];

                if (word.Length < 3)
                    continue;

                if (b.Contains(word, StringComparison.InvariantCulture))
                {
                    double wordWeight = word.Length / (double)a.Length;
                    score += wordWeight;
                }
            }

            return score;
        }

        /// <summary>
        /// Splits strings into words and returns a score 0 to 1 of how many words they had in common
        /// </summary>
        public static double WordMatch(string a, string b)
        {
            double scoreA = ContainsWords(a, b);
            double scoreB = ContainsWords(b, a);

            double score = Math.Max(scoreA, scoreB);

            return score;
        }
    }
}
