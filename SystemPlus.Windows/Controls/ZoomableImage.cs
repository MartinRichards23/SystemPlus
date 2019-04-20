using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace SystemPlus.Windows.Controls
{
    public class ZoomableImage : Image
    {
        Point start;

        readonly ScaleTransform scale;
        readonly TranslateTransform translate;

        public ZoomableImage()
        {
            RenderTransformOrigin = new Point(0.5, 0.5);

            TransformGroup group = new TransformGroup();
            scale = new ScaleTransform();
            translate = new TranslateTransform();
            group.Children.Add(scale);

            group.Children.Add(translate);

            RenderTransform = group;
            MouseWheel += ZoomableImage_MouseWheel;
            MouseLeftButtonDown += ZoomableImage_MouseLeftButtonDown;
            MouseLeftButtonUp += ZoomableImage_MouseLeftButtonUp;
            MouseMove += ZoomableImage_MouseMove;
        }

        void ZoomableImage_MouseMove(object sender, MouseEventArgs e)
        {
            if (!IsMouseCaptured)
                return;

            Vector v = start - e.GetPosition(this);
            translate.X -= v.X;
            translate.Y -= v.Y;
        }

        void ZoomableImage_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            ReleaseMouseCapture();
        }

        void ZoomableImage_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            CaptureMouse();

            start = e.GetPosition(this);
        }

        void ZoomableImage_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            double change = e.Delta > 0 ? 1.05 : 0.95;

            double newZoom = scale.ScaleX;
            newZoom *= change;
            newZoom = MathTools.Clip(newZoom, 0.5, 10);

            scale.ScaleX = newZoom;
            scale.ScaleY = newZoom;
            scale.CenterX = 0.5;
            scale.CenterY = 0.5;
        }
    }
}