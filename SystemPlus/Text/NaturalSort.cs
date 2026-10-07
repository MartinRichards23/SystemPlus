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
            if (x == y)
                return 0;

            if (x == null)
                x = string.Empty;

            if (y == null)
                y = string.Empty;

            string[] x1 = Regex.Split(x.Replace(" ", "", StringComparison.InvariantCulture), "([0-9]+)");
            string[] y1 = Regex.Split(y.Replace(" ", "", StringComparison.InvariantCulture), "([0-9]+)");

            for (int i = 0; i < x1.Length && i < y1.Length; i++)
            {
                if (x1[i] != y1[i])
                    return PartCompare(x1[i], y1[i]);
            }

            return x1.Length.CompareTo(y1.Length);
        }

        private static int PartCompare(string left, string right)
        {
            if (left.Length == 0 || right.Length == 0 || !char.IsAsciiDigit(left[0]) || !char.IsAsciiDigit(right[0]))
                return string.Compare(left, right, StringComparison.InvariantCulture);

            // compare digit runs numerically without parsing, so any length is supported
            string l = left.TrimStart('0');
            string r = right.TrimStart('0');

            if (l.Length != r.Length)
                return l.Length.CompareTo(r.Length);

            int result = string.CompareOrdinal(l, r);
            if (result != 0)
                return result;

            return left.Length.CompareTo(right.Length);
        }

    }
}
