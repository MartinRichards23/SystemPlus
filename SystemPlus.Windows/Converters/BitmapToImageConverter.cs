using System;
using System.Globalization;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace SystemPlus.Windows.Converters
{
    /// <summary>
    /// Converts BitmapSource to an Image
    /// </summary>
    [ValueConversion(typeof(BitmapSource), typeof(Image))]
    public class BitmapToImageConverter : IValueConverter
    {
        public double Width { get; set; } = -1;
        public double Height { get; set; } = -1;

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            BitmapSource? bms = value as BitmapSource;

            if (bms == null)
                return null;

            Image img = new Image
            {
                Source = bms
            };

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