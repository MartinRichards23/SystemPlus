using System;
using System.Collections.Generic;
using System.Windows;

namespace SystemPlus.Windows
{
    public static class VectorTools
    {
        /// <summary>
        /// Calculates the distance between two points
        /// </summary>
        public static double Distance(Point a, Point b)
        {
            return Math.Sqrt(Math.Pow(b.X - a.X, 2) + Math.Pow(b.Y - a.Y, 2));
        }

        /// <summary>
        /// Calculates the midpoint of 2 given points
        /// </summary>
        public static Point MidPoint(Point a, Point b)
        {
            return new Point((a.X + b.X) / 2, (a.Y + b.Y) / 2);
        }

        public static Point Lerp(Point v0, Point v1, double t)
        {
            double x = (1 - t) * v0.X + t * v1.X;
            double y = (1 - t) * v0.Y + t * v1.Y;

            return new Point(x, y);
        }

        public static double Lerp(double v0, double v1, double t)
        {
            return (1 - t) * v0 + t * v1;
        }

        /// <summary>
        /// Calculates the closest distance between a line and a point
        /// </summary>
        public static double DistanceToLine(Point lineStart, Point lineEnd, Point point)
        {
            double A = point.X - lineStart.X;
            double B = point.Y - lineStart.Y;
            double C = lineEnd.X - lineStart.X;
            double D = lineEnd.Y - lineStart.Y;

            double dot = A * C + B * D;
            double len_sq = C * C + D * D;
            double param = dot / len_sq;

            Point closest;

            if (param < 0)
                closest = lineStart;
            else if (param > 1)
                closest = lineEnd;
            else
            {
                double xx = lineStart.X + param * C;
                double yy = lineStart.Y + param * D;

                closest = new Point(xx, yy);
            }

            // we now have the closest point, find the distance to that
            return Distance(point, closest);
        }

        /// <summary>
        /// Gets the bounding box of a collection of points
        /// </summary>
        public static Rect GetBounds(this IEnumerable<Point> points)
        {
            double top = double.MaxValue;
            double bottom = double.MinValue;
            double left = double.MaxValue;
            double right = double.MinValue;

            foreach (Point point in points)
            {
                if (point.Y < top)
                    top = point.Y;
                if (point.Y > bottom)
                    bottom = point.Y;

                if (point.X < left)
                    left = point.X;
                if (point.X > right)
                    right = point.X;
            }

            if (bottom < top || right < left)
                return new Rect();

            return new Rect(left, top, right - left, bottom - top);
        }

        /// <summary>
        /// Gets the bounding box of a collection of rects
        /// </summary>
        public static Rect GetBounds(this IEnumerable<Rect> rects)
        {
            double top = double.MaxValue;
            double bottom = double.MinValue;
            double left = double.MaxValue;
            double right = double.MinValue;

            foreach (Rect rect in rects)
            {
                if (rect.Top < top)
                    top = rect.Top;
                if (rect.Bottom > bottom)
                    bottom = rect.Bottom;

                if (rect.Left < left)
                    left = rect.Left;
                if (rect.Right > right)
                    right = rect.Right;
            }

            if (bottom < top || right < left)
                return new Rect();

            return new Rect(left, top, right - left, bottom - top);
        }

        public static Point RoundDownPoint(Point point)
        {
            return new Point(Math.Floor(point.X), Math.Floor(point.Y));
        }

        public static Point RoundUpPoint(Point point)
        {
            return new Point(Math.Ceiling(point.X), Math.Ceiling(point.Y));
        }

        public static bool IntersectsLineSegment(Rect rect, Point p1, Point p2)
        {
            if (p1.X == p2.X)
                return (rect.Left <= p1.X && p1.X <= rect.Right && Math.Min(p1.Y, p2.Y) <= rect.Bottom && Math.Max(p1.Y, p2.Y) >= rect.Top);
            if (p1.Y == p2.Y)
                return (rect.Top <= p1.Y && p1.Y <= rect.Bottom && Math.Min(p1.X, p2.X) <= rect.Right && Math.Max(p1.X, p2.X) >= rect.Left);
            if (Contains(rect, p1))
                return true;
            if (Contains(rect, p2))
                return true;
            if (IntersectingLines(new Point(rect.Left, rect.Top), new Point(rect.Right, rect.Top), p1, p2))
                return true;
            if (IntersectingLines(new Point(rect.Right, rect.Top), new Point(rect.Right, rect.Bottom), p1, p2))
                return true;
            if (IntersectingLines(new Point(rect.Right, rect.Bottom), new Point(rect.Left, rect.Bottom), p1, p2))
                return true;
            if (IntersectingLines(new Point(rect.Left, rect.Bottom), new Point(rect.Left, rect.Top), p1, p2))
                return true;
            return false;
        }

        public static bool Contains(Rect a, Point b)
        {
            return a.X <= b.X && b.X <= a.X + a.Width && a.Y <= b.Y && b.Y <= a.Y + a.Height;
        }

        public static bool IntersectingLines(Point a1, Point a2, Point b1, Point b2)
        {
            return ((ComparePointWithLine(a1, a2, b1) * ComparePointWithLine(a1, a2, b2) <= 0) && (ComparePointWithLine(b1, b2, a1) * ComparePointWithLine(b1, b2, a2) <= 0));
        }

        public static int ComparePointWithLine(Point a1, Point a2, Point p)
        {
            double x2 = a2.X - a1.X;
            double y2 = a2.Y - a1.Y;
            double px = p.X - a1.X;
            double py = p.Y - a1.Y;
            double ccw = px * y2 - py * x2;
            if (ccw == 0)
            {
                ccw = px * x2 + py * y2;
                if (ccw > 0)
                {
                    px -= x2;
                    py -= y2;
                    ccw = px * x2 + py * y2;
                    if (ccw < 0)
                        ccw = 0;
                }
            }
            return (ccw < 0) ? -1 : ((ccw > 0) ? 1 : 0);
        }
        
        /// <summary>
        /// Returns the dpi of the current environment
        /// </summary>
        public static Vector GetDpi()
        {
            PresentationSource source = PresentationSource.FromVisual(Application.Current.MainWindow);

            double dpiX = 96.0 * source.CompositionTarget.TransformToDevice.M11;
            double dpiY = 96.0 * source.CompositionTarget.TransformToDevice.M22;

            return new Vector(dpiX, dpiY);
        }
        
        /// <summary>
        /// Clips a size's width and height
        /// </summary>
        public static Size ClipSize(Size value, Size max)
        {
            if (value.Width > max.Width)
                value = new Size(max.Width, value.Height * (max.Width / value.Width));

            if (value.Height > max.Height)
                value = new Size(value.Width * (max.Height / value.Height), max.Height);

            return value;
        }
    }
}