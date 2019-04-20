using System.Windows;

namespace SystemPlus.Windows.Media
{
    public class GridMaker : Pointmaker
    {
        #region Fields

        readonly int gridWidth;
        readonly Size spacing;

        int count;

        #endregion

        public GridMaker(Point origin, int gridWidth, Size spacing)
            : base(origin)
        {
            this.gridWidth = gridWidth;
            this.spacing = spacing;
        }

        public int GridWidth
        {
            get { return gridWidth; }
        }

        public Size Spacing
        {
            get { return spacing; }
        }

        public override Point NextPoint()
        {
            int x = count % gridWidth;
            int y = (count - x) / gridWidth;
            count++;

            Point p = new Point(Origin.X + (x * spacing.Width), Origin.Y + (y * spacing.Height));
            return p;
        }
    }
}