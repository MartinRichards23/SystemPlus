namespace SystemPlus
{
    /// <summary>
    /// Extension methods for Random
    /// </summary>
    public static class RandomExtensions
    {
        /// <summary>
        /// Make a random string of specified length
        /// </summary>
        public static string NextString(this Random rand, int size)
        {
            if (rand == null)
                throw new ArgumentNullException(nameof(rand));

            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < size; i++)
            {
                char ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * rand.NextDouble() + 65)));
                builder.Append(ch);
            }

            return builder.ToString();
        }

        /// <summary>
        /// Returns a random bool
        /// </summary>
        public static bool NextBool(this Random rand, double trueChance = 0.5)
        {
            if (rand == null)
                throw new ArgumentNullException(nameof(rand));

            return rand.NextDouble() < trueChance;
        }

        /// <summary>
        /// Returns a random timespan
        /// </summary>
        public static TimeSpan NextTimespan(this Random rand, TimeSpan min, TimeSpan max)
        {
            long ticks = rand.NextLong(min.Ticks, max.Ticks);
            return new TimeSpan(ticks);
        }

        /// <summary>
        /// Returns a random long
        /// </summary>
        public static long NextLong(this Random rand)
        {
            if (rand == null)
                throw new ArgumentNullException(nameof(rand));

            byte[] buffer = new byte[8];
            rand.NextBytes(buffer);
            return BitConverter.ToInt64(buffer, 0);
        }

        public static long NextLong(this Random rand, long min, long max)
        {
            EnsureMinLEQMax(ref min, ref max);
            long numbersInRange = unchecked(max - min + 1);
            if (numbersInRange < 0)
                throw new ArgumentException("Size of range between min and max must be less than or equal to Int64.MaxValue");

            long randomOffset = NextLong(rand);
            if (IsModuloBiased(randomOffset, numbersInRange))
                return NextLong(rand, min, max); // Try again
            return min + PositiveModuloOrZero(randomOffset, numbersInRange);
        }

        /// <summary>
        /// Returns a random number between 0 and 1
        /// </summary>
        public static float NextFloat(this Random rand)
        {
            if (rand == null)
                throw new ArgumentNullException(nameof(rand));

            double d = rand.NextDouble();
            return (float)d;
        }

        /// <summary>
        /// Returns a random enum value
        /// </summary>
        public static T NextEnumValue<T>(this Random rand) where T : struct
        {
            if (rand == null)
                throw new ArgumentNullException(nameof(rand));

            Array values = Enum.GetValues(typeof(T));
            int index = rand.Next(values.Length);
            object? result = values.GetValue(index);

            if (result == null)
                throw new NullReferenceException(nameof(result));

            return (T)result;
        }

        static bool IsModuloBiased(long randomOffset, long numbersInRange)
        {
            long greatestCompleteRange = numbersInRange * (long.MaxValue / numbersInRange);
            return randomOffset > greatestCompleteRange;
        }

        static long PositiveModuloOrZero(long dividend, long divisor)
        {
            Math.DivRem(dividend, divisor, out long mod);

            if (mod < 0)
                mod += divisor;

            return mod;
        }

        static void EnsureMinLEQMax(ref long min, ref long max)
        {
            if (min <= max)
                return;
            long temp = min;
            min = max;
            max = temp;
        }
    }
}