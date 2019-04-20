using System;
using System.Globalization;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace SystemPlus.Windows.Converters
{
    [ValueConversion(typeof(BitmapSource), typeof(Image))]
    public class BitmapToImageConverter : IValueConverter
    {
        double width = -1;
        double height = -1;

        public double Width
        {
            get { return width; }
            set { width = value; }
        }

        public double Height
        {
            get { return height; }
            set { height = value; }
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            BitmapSource bms = value as BitmapSource;

            if (bms == null)
                return null;

            Image img = new Image();
            img.Source = bms;

            if (Width > 0)
                img.Width = Width;
            if (Height > 0)
                img.Height = Height;

            return img;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}