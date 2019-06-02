using System.IO;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media.Imaging;
using SystemPlus.Windows.Media;

namespace SystemPlus.Windows
{
    public static class ClipboardTools
    {
        /// <summary>
        /// Returns true if dataobject contains image data
        /// </summary>
        public static bool ContainsImage(this IDataObject data)
        {
            try
            {
                if (data.GetData(typeof(BitmapSource)) != null)
                    return true;

                if (data.GetDataPresent(typeof(BitmapSource)))
                    return true;

                if (data.GetDataPresent(DataFormats.Dib, true))
                    return true;

                if (data.GetDataPresent(DataFormats.Bitmap, true))
                    return true;

                return false;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Gets the image from the IDataObject object
        /// </summary>
        public static BitmapSource GetImage(this IDataObject data)
        {
            //string[] a = data.GetFormats();

            //if (data.GetDataPresent(typeof(BitmapSource)))
            {
                BitmapSource bms = data.GetData(typeof(BitmapSource)) as BitmapSource;

                if (bms != null)
                    return bms;
            }

            if (data.GetDataPresent(DataFormats.Dib, true))
            {
                using (MemoryStream ms = (MemoryStream)data.GetData(DataFormats.Dib))
                {
                    byte[] dib = ms.ToArray();
                    BitmapSource bms = LegacyImageTools.BitmapSourceFromDib(dib);

                    if (bms != null)
                        return bms;
                }
            }

            if (data.GetDataPresent(DataFormats.Bitmap, true))
            {
                InteropBitmap bmp = data.GetData(DataFormats.Bitmap) as InteropBitmap;
                return bmp;
            }

            return null;
        }
    }
}