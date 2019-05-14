using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SystemPlus
{
    /// <summary>
    /// Various statistics tools
    /// </summary>
    public class Statistics
    {
        public static double CalcStandardDeviation(IEnumerable<double> doubleList)
        {
            return Math.Sqrt(CalcVariance(doubleList));
        }

        public static double CalcVariance(IEnumerable<double> doubleList)
        {
            double average = 0;
            int count = 0;
            double sumOfDerivation = 0;

            foreach (double value in doubleList)
            {
                average += value;
                count++;
                sumOfDerivation += (value) * (value);
            }

            average = average / count;

            double sumOfDerivationAverage = sumOfDerivation / count;
            return sumOfDerivationAverage - (average * average);
        }

    }
}
