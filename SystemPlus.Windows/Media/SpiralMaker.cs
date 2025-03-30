using System.Windows;

namespace SystemPlus.Windows.Media
{
    public class SpiralMaker : Pointmaker
    {
        #region Fields

        double theta;
        double currentRadius;
        double currentAngle;

        double startRadius;
        double startAngle = 35;

        double radiusSpacing;
        double circumferenceSpacing;

        #endregion

        public SpiralMaker(Point origin, double startRadius, double radiusSpacing, double circumferenceSpacing)
            : base(origin)
        {
            theta = 0;

            this.startRadius = startRadius;
            this.radiusSpacing = radiusSpacing;
            this.circumferenceSpacing = circumferenceSpacing;

            currentAngle = startAngle;
            currentRadius = startRadius;
        }

        #region Properties

        public double StartRadius
        {
            get { return startRadius; }
            set { startRadius = value; }
        }

        public double StartAngleChange
        {
            get { return startAngle; }
            set { startAngle = value; }
        }

        /// <summary>
        /// The distance to move out each revolution
        /// </summary>
        public double RadiusSpacing
        {
            get { return radiusSpacing; }
            set { radiusSpacing = value; }
        }

        /// <summary>
        /// The space between neighbouring items
        /// </summary>
        public double CircumferenceSpacing
        {
            get { return circumferenceSpacing; }
            set { circumferenceSpacing = value; }
        }

        #endregion

        public override Point NextPoint()
        {
            double x = Origin.X + ((Math.Cos(theta) * currentRadius));
            double y = Origin.Y + ((Math.Sin(theta) * currentRadius));
            theta += MathTools.DegreesToRadians(currentAngle);

            double nodesPerRev = 360 / currentAngle;
            currentRadius += radiusSpacing / nodesPerRev;

            double circumference = 2 * Math.PI * currentRadius;
            double num = circumference / circumferenceSpacing;
            currentAngle = 360 / num;

            return new Point(x, y);
        }
    }
}