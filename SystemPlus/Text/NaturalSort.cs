using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace SystemPlus.Text
{
    public class NaturalSort : IComparer<string>
    {
        /// <summary>
        /// Sorts strings in natural way, e.g. file 1, file 2, file 3, file 25
        /// </summary>
        public int Compare(string? x, string? y)
        {
            Dictionary<string, string[]> table = new Dictionary<string, string[]>();
            bool isAscending = false;

            if (x == y)
                return 0;

            if (x == null)
                x = string.Empty;

            if (y == null)
                y = string.Empty;

            if (!table.TryGetValue(x, out string[]? x1))
            {
                x1 = Regex.Split(x.Replace(" ", "", StringComparison.InvariantCulture), "([0-9]+)");
                table.Add(x, x1);
            }

            if (!table.TryGetValue(y, out string[]? y1))
            {
                y1 = Regex.Split(y.Replace(" ", "", StringComparison.InvariantCulture), "([0-9]+)");
                table.Add(y, y1);
            }

            int returnVal;

            for (int i = 0; i < x1.Length && i < y1.Length; i++)
            {
                if (x1[i] != y1[i])
                {
                    returnVal = PartCompare(x1[i], y1[i]);
                    return isAscending ? -returnVal : returnVal;
                }
            }

            if (y1.Length > x1.Length)
                returnVal = -1;
            else if (x1.Length > y1.Length)
                returnVal = 1;
            else
                returnVal = 0;

            return isAscending ? returnVal : -returnVal;
        }

        private static int PartCompare(string left, string right)
        {
            if (!int.TryParse(left, out int x))
                return string.Compare(left, right, StringComparison.InvariantCulture);

            if (!int.TryParse(right, out int y))
                return string.Compare(left, right, StringComparison.InvariantCulture);

            return x.CompareTo(y);
        }

    }
}
