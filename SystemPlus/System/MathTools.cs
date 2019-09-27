using System;

namespace SystemPlus
{
    /// <summary>
    /// Useful extra tools for the Math class
    /// </summary>
    public static class MathTools
    {
        public const double TwoPi = Math.PI * 2;
        public const double Pi180 = 180.0 / Math.PI;
        public const double PiOver180 = Math.PI / 180;

        /// <summary>
        /// Returns a value between 0 and 1
        /// </summary>
        public static double Sigmoid(double x)
        {
            return 1 / (1 + Math.Exp(-x));
        }

        /// <summary>
        /// Turns North to South, East to West etc.
        /// </summary>
        /// <param name="bearing">Bearing</param>
        /// <returns></returns>
        public static double ReverseBearing(double bearing)
        {
            return (bearing + 180) % 360;
        }

        public static double DegreesToRadians(double degs)
        {
            return degs * PiOver180;
        }

        public static float DegreesToRadians(float degs)
        {
            return degs * (float)PiOver180;
        }

        public static double RadianToDegree(double rads)
        {
            return rads * Pi180;
        }

        public static double RadianToDegreeBearing(double rads)
        {
            return ((rads * Pi180) + 360) % 360;
        }

        /// <summary>
        /// Ensures angle is between 0 and 360, e.g. 361 will return 1.
        /// </summary>
        public static double ClampAngle(double angle)
        {
            angle = angle % 360;

            while (angle < 0)
            {
                angle += 360;
            }

            return angle;
        }

        public static double Hypotenuse(double side1, double side2)
        {
            return Math.Sqrt(side1 * side1 + side2 * side2);
        }

        public static double Clip(double value, double minValue, double maxValue)
        {
            return Math.Min(Math.Max(value, minValue), maxValue);
        }

        public static int Clip(int value, int minValue, int maxValue)
        {
            return Math.Min(Math.Max(value, minValue), maxValue);
        }

        /// <summary>
        /// if d is 8.654 returns 0.654
        /// </summary>
        public static double GetFraction(double d)
        {
            return d - Math.Floor(d);
        }

        public static double RootN(double a, int n)
        {
            return Math.Pow(a, 1.0 / n);
        }

        /// <summary>
        /// Gets proportion of value given max and min values
        /// </summary>
        public static double Proportion(double min, double max, double value)
        {
            return (value - min) / (max - min);
        }

        /// <summary>
        /// Round up to nearest 10
        /// </summary>
        public static int RoundUp(int value)
        {
            return 10 * ((value + 9) / 10);
        }

        /// <summary>
        /// Round down to nearest 10
        /// </summary>
        public static int RoundDown(int value)
        {
            return 10 * (value / 10);
        }
    }
}