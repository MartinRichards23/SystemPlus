using System.Windows;

namespace SystemPlus.Windows.Media
{
    /// <summary>
    /// Base class for PointMakers
    /// </summary>
    public abstract class Pointmaker
    {
        protected Pointmaker(Point origin)
        {
            this.Origin = origin;
        }

        protected Point Origin { get; }

        public abstract Point NextPoint();
    }
}