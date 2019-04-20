using System;
using System.Windows;
using System.Windows.Media;

namespace SystemPlus.Windows.Media
{
    public static class DrawingExtensions
    {
        /// <summary>
        /// Draws text, simple but not efficient
        /// </summary>
        public static void DrawText(this DrawingContext dc, string text, double size, Brush brush, Point origin)
        {
            Typeface typeface = new Typeface("Arial");
            
            if (!typeface.TryGetGlyphTypeface(out GlyphTypeface glyphTypeface))
                throw new InvalidOperationException("No glyph typeface found");

            ushort[] glyphIndexes = new ushort[text.Length];
            double[] advanceWidths = new double[text.Length];

            for (int n = 0; n < text.Length; n++)
            {
                ushort glyphIndex = (ushort)(text[n] - 29);
                glyphIndexes[n] = glyphIndex;
                advanceWidths[n] = glyphTypeface.AdvanceWidths[glyphIndex] * size;
            }

            GlyphRun glyphRun = new GlyphRun(glyphTypeface, 0, false, size, glyphIndexes, origin, advanceWidths, null, null, null, null, null, null);

            dc.DrawGlyphRun(brush, glyphRun);
        }
    }
}