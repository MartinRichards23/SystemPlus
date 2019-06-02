using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;

namespace SystemPlus.Windows
{
    /// <summary>
    /// 
    /// </summary>
    public class DrawingCanvas : Canvas
    {
        #region Fields

        protected Rect viewPort;

        protected readonly ScaleTransform scale = new ScaleTransform();
        protected readonly TranslateTransform translate = new TranslateTransform();

        protected DateTime lastDraw = DateTime.UtcNow;

        #endregion

        public DrawingCanvas()
        {
            IsHitTestVisible = false;

            TransformGroup transforms = new TransformGroup();
            transforms.Children.Add(translate);
            transforms.Children.Add(scale);

            //RenderTransformOrigin = new Point(0.5, 0.5);
            RenderTransform = transforms;

            if (DesignerProperties.GetIsInDesignMode(this))
                return;

            DispatcherTimer timer = new DispatcherTimer(DispatcherPriority.Render)
            {
                Interval = TimeSpan.FromMilliseconds(16)
            };
            timer.Tick += timer_Tick;
            timer.Start();
        }

        protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
        {
            base.OnRenderSizeChanged(sizeInfo);

            scale.CenterX = ActualWidth / 2;
            scale.CenterY = ActualHeight / 2;
            Invalidate();
        }

        #region Properties

        /// <summary>
        /// Current zoom level
        /// </summary>
        public double Zoom
        {
            get { return scale.ScaleX; }
            set
            {
                scale.ScaleX = value;
                scale.ScaleY = value;
                Invalidate();
            }
        }

        /// <summary>
        /// Current top left of the viewport
        /// </summary>
        public Point Position
        {
            get { return new Point(translate.X, translate.Y); }
            set
            {
                translate.X = value.X;
                translate.Y = value.Y;
                Invalidate();
            }
        }

        /// <summary>
        /// Current centre of the viewport
        /// </summary>
        public Point Centre
        {
            get { return new Point((ActualWidth / 2) - translate.X, (ActualHeight / 2) - translate.Y); }
            set
            {
                translate.X = (ActualWidth / 2) - value.X;
                translate.Y = (ActualHeight / 2) - value.Y;
                Invalidate();
            }
        }

        public Rect ViewPort
        {
            get { return viewPort; }
        }

        #endregion

        /// <summary>
        /// Causes the canvas to render
        /// </summary>
        public void Invalidate()
        {
            InvalidateVisual();
        }

        void timer_Tick(object sender, EventArgs e)
        {
            Invalidate();
        }

        public Point ToViewCoordinates(Point point)
        {
            return RenderTransform.Transform(point);
        }

        public Point ToWorldCoordinates(Point point)
        {
            return RenderTransform.Inverse.Transform(point);
        }

        public void UpdateViewPortRect()
        {
            // calc the view port window
            viewPort = new Rect(0, 0, ActualWidth, ActualHeight);
            viewPort = RenderTransform.TransformBounds(viewPort);

            Point topLeft = RenderTransform.Inverse.Transform(new Point(0, 0));
            Point bottomRight = RenderTransform.Inverse.Transform(new Point(ActualWidth, ActualHeight));

            Rect r = new Rect(topLeft, bottomRight);

            viewPort = r;

        }
    }
}