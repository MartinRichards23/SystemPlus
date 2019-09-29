using System;
using System.Collections.Generic;
using System.Windows.Media;

namespace SystemPlus.Windows.Media
{
    /// <summary>
    /// Colour tools and extensions
    /// </summary>
    public static class ColourTools
    {
        public static int Brightness(this Color c)
        {
            return (int)Math.Sqrt(c.R * c.R * .241 + c.G * c.G * .691 + c.B * c.B * .068);
        }

        public static Color AdjustBrightness(this Color c, int change)
        {
            int r = c.R + change;
            int g = c.G + change;
            int b = c.B + change;

            return Color.FromArgb(c.A, (byte)r, (byte)g, (byte)b);
        }

        public static string ColorToHtml(this Color col)
        {
            return string.Format("rgba({0},{1},{2},{3:f2})", col.R, col.G, col.B, col.ScA);
        }

        public static string ColorToHexRgb(this Color col)
        {
            return string.Format("#{0:X2}{1:X2}{2:X2}", col.R, col.G, col.B);
        }

        public static Color ColorFromText(string input)
        {
            return (Color)ColorConverter.ConvertFromString(input);
        }

        public static Color ColorFromText(string input, Color defaultColour)
        {
            try
            {
                object col = ColorConverter.ConvertFromString(input);

                if (col is Color)
                    return (Color)col;

                return defaultColour;
            }
            catch
            {
                return defaultColour;
            }
        }

        public static Color ColorFromInt(int input)
        {
            byte[] bytes = BitConverter.GetBytes(input);
            return Color.FromArgb(bytes[3], bytes[2], bytes[1], bytes[0]);
        }

        public static int ColorToInt(this Color input)
        {
            return BitConverter.ToInt32(new[] { input.B, input.G, input.R, input.A }, 0);
        }

        public static int? ColorToInt(this Color? input)
        {
            if (input == null)
                return null;

            return ColorToInt((Color)input);
        }

        public static Color? ColorFromInt(int? input)
        {
            if (input == null)
                return null;

            return ColorFromInt((int)input);
        }

        public static Color[] GetHighlightColors()
        {
            List<Color> cols = new List<Color>
            {

                // http://msdn.microsoft.com/en-us/library/system.windows.media.colors.aspx

                Colors.Orange,
                Colors.LightPink,
                Colors.SkyBlue,
                Colors.Violet,
                Colors.PaleGreen,
                Colors.Fuchsia,
                Colors.Yellow,
                Colors.Cyan,
                Colors.Plum,
                Colors.Turquoise,
                Colors.YellowGreen,
                Colors.LawnGreen,
                Colors.Goldenrod,
                Colors.DodgerBlue
            };

            return cols.ToArray();
        }

        public static Color Average(Color a, Color b)
        {
            return Average(new[] { a, b });
        }

        /// <summary>
        /// Calculates the average colour from a collection of colours
        /// </summary>
        public static Color Average(IEnumerable<Color> colours)
        {
            int count = 0;
            int a = 0;
            int r = 0;
            int g = 0;
            int b = 0;

            foreach (Color col in colours)
            {
                count++;
                a += col.A;
                r += col.R;
                g += col.G;
                b += col.B;
            }

            a /= count;
            r /= count;
            g /= count;
            b /= count;

            return Color.FromArgb((byte)a, (byte)r, (byte)g, (byte)b);
        }

        /// <summary>
        /// Gets a colour from rainbow at postion relative to min/max
        /// </summary>
        public static Color GetRainbowColor(double value, double min, double max)
        {
            const double quart = 0.25f;
            const double half = 0.5f;
            const double threequart = 0.75f;

            value = (value - min) / (max - min);

            if (value < quart) //high
            {
                double c = value / quart;
                return Color.FromRgb(255, (byte)(c * 255), 0);
            }
            if (value < half) //middle
            {
                double c = (value - quart) / quart;
                return Color.FromRgb((byte)(255 - (c * 255)), 255, 0);
            }
            if (value < threequart) //middle
            {
                double c = (value - half) / quart;
                return Color.FromRgb(0, 255, (byte)(c * 255));
            }
            else //low
            {
                double c = (value - threequart) / quart;
                return Color.FromRgb(0, (byte)(255 - (c * 255)), 255);
            }
        }
    }
}