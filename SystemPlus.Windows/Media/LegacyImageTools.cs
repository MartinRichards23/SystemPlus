using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media.Imaging;
using SystemPlus.Windows.NativeUtilities;
using Drawing = System.Drawing;

namespace SystemPlus.Windows.Media
{
    /// <summary>
    /// Functions for working with bitmaps
    /// </summary>
    public static class LegacyImageTools
    {
        /// <summary>
        /// Gets a bitmapsource from Device independent bitmap data
        /// </summary>
        public static BitmapSource BitmapSourceFromDib(byte[] dib)
        {
            int width = BitConverter.ToInt32(dib, 4);
            int height = BitConverter.ToInt32(dib, 8);
            short bpp = BitConverter.ToInt16(dib, 14);

            if (bpp != 32)
            {
                // Invalid pixel format
                return null;
            }

            GCHandle gch = GCHandle.Alloc(dib, GCHandleType.Pinned);
            Drawing.Bitmap bmp = null;

            try
            {
                IntPtr ptr = new IntPtr((long)gch.AddrOfPinnedObject() + 40);
                bmp = new Drawing.Bitmap(width, height, width * 4, Drawing.Imaging.PixelFormat.Format32bppArgb, ptr);
                bmp.RotateFlip(Drawing.RotateFlipType.RotateNoneFlipY);

                return BitmapSourceFromBmp(bmp);
            }
            finally
            {
                gch.Free();
                if (bmp != null) bmp.Dispose();
            }
        }

        public static BitmapSource BitmapSourceFromBmp(Drawing.Bitmap bmp)
        {
            IntPtr obj = bmp.GetHbitmap();
            BitmapSource bms;
            try
            {
                bms = Imaging.CreateBitmapSourceFromHBitmap(obj, IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
            }
            finally
            {
                NativeMethods.DeleteObject(obj);
            }

            return bms;
        }

        public static Drawing.Bitmap BmpFromBitmapSource(BitmapSource bms)
        {
            Drawing.Bitmap bmp = new Drawing.Bitmap(bms.PixelWidth, bms.PixelHeight, Drawing.Imaging.PixelFormat.Format32bppPArgb);

            Drawing.Imaging.BitmapData data = bmp.LockBits(new Drawing.Rectangle(Drawing.Point.Empty, bmp.Size), Drawing.Imaging.ImageLockMode.WriteOnly, Drawing.Imaging.PixelFormat.Format32bppPArgb);

            bms.CopyPixels(Int32Rect.Empty, data.Scan0, data.Height * data.Stride, data.Stride);
            bmp.UnlockBits(data);

            return bmp;
        }

        public static Drawing.Bitmap BmpFromBytes(byte[] data)
        {
            using (MemoryStream ms = new MemoryStream(data))
            {
                return new Drawing.Bitmap(ms);
            }
        }

        public static Drawing.Bitmap BmpFromString(string data)
        {
            byte[] bytes = Convert.FromBase64String(data);
            return BmpFromBytes(bytes);
        }

        public static byte[] ToBytes(this Drawing.Bitmap bmp)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                bmp.Save(ms, Drawing.Imaging.ImageFormat.Jpeg);
                return ms.ToArray();
            }
        }
    }
}