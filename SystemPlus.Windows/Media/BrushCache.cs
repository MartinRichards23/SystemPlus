﻿using System.Collections.Generic;
using System.Windows.Media;

namespace SystemPlus.Windows.Media
{
    /// <summary>
    /// Caches SolidColorBrushes
    /// Only use when the same colour brush is created repeatedly
    /// </summary>
    public static class BrushCache
    {
        static readonly IDictionary<Color, SolidColorBrush> brushCache = new Dictionary<Color, SolidColorBrush>();
        static readonly IDictionary<string, Pen> penCache = new Dictionary<string, Pen>();

        /// <summary>
        /// Gets a frozen solid colour brush (creates it if not already cached)
        /// </summary>
        public static SolidColorBrush GetBrush(Color col)
        {
            lock (brushCache)
            {
                if (brushCache.ContainsKey(col))
                    return brushCache[col];

                SolidColorBrush brush = new SolidColorBrush(col);
                brush.Freeze();

                brushCache.Add(col, brush);

                return brush;
            }
        }

        public static Pen GetPen(Color col, double thickness)
        {
            lock (penCache)
            {
                string key = string.Format("{0} {1} {2} {4}", col.A, col.R, col.G, col.B, thickness);

                if (penCache.ContainsKey(key))
                    return penCache[key];

                SolidColorBrush brush = GetBrush(col);

                Pen pen = new Pen(brush, thickness);
                pen.Freeze();

                penCache.Add(key, pen);

                return pen;
            }
        }
    }
}