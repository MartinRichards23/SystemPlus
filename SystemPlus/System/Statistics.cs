namespace SystemPlus
{
    /// <summary>
    /// Various statistics helpers
    /// </summary>
    public static class Statistics
    {
        public static double CalcStandardDeviation(IEnumerable<double> doubleList)
        {
            return Math.Sqrt(CalcVariance(doubleList));
        }

        public static double CalcVariance(IEnumerable<double> doubleCollection)
        {
            if (doubleCollection == null)
                throw new ArgumentNullException(nameof(doubleCollection));

            double mean = 0;
            double m2 = 0;
            int count = 0;

            // Welford's algorithm, numerically stable
            foreach (double value in doubleCollection)
            {
                count++;
                double delta = value - mean;
                mean += delta / count;
                m2 += delta * (value - mean);
            }

            if (count == 0)
                throw new ArgumentException("Collection must contain at least one value", nameof(doubleCollection));

            return m2 / count;
        }

    }
}
