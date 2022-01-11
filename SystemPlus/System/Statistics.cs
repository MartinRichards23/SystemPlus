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

            double average = 0;
            int count = 0;
            double sumOfDerivation = 0;

            foreach (double value in doubleCollection)
            {
                average += value;
                count++;
                sumOfDerivation += (value) * (value);
            }

            average /= count;

            double sumOfDerivationAverage = sumOfDerivation / count;
            return sumOfDerivationAverage - (average * average);
        }

    }
}
