using System.Windows;

namespace SystemPlus.Windows.Media
{
    /// <summary>
    /// Base class for PointMakers
    /// </summary>
    public abstract class Pointmaker
    {
        readonly Point origin;

        protected Pointmaker(Point origin)
        {
            this.origin = origin;
        }

        protected Point Origin
        {
            get { return origin; }
        }

        public abstract Point NextPoint();
    }

    public class BasicPointMaker : Pointmaker
    {
        Point offset;

        public BasicPointMaker(Point origin, Point offset)
            : base(origin)
        {
            Offset = offset;
        }

        public Point Offset
        {
            get { return offset; }
            set { offset = value; }
        }

        public override Point NextPoint()
        {
            Point p = new Point(Origin.X + Offset.X, Origin.Y + Offset.Y);
            return p;
        }
    }
}