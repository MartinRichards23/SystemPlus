﻿using System.IO;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using SystemPlus.IO;

namespace SystemPlus.Windows.Media
{
    /// <summary>
    /// Functions for working with bitmaps
    /// </summary>
    public static class ImageTools
    {
        /// <summary>
        /// Writes the bitmap to a stream
        /// </summary>
        public static void Write(BitmapSource bms, BitmapEncoder encoder, Stream stream)
        {
            BitmapFrame bmf = BitmapFrame.Create(bms);
            encoder.Frames.Add(bmf);
            encoder.Save(stream);
        }

        /// <summary>
        /// Writes a bitmap to the given filepath
        /// </summary>
        public static void Write(BitmapSource bms, BitmapEncoder encoder, string path)
        {
            // make sure directory exists
            string dir = Path.GetDirectoryName(Path.GetFullPath(path));

            FileSystem.EnsureDirExists(dir);

            using (FileStream fs = File.Create(path, 1024, FileOptions.Asynchronous))
            {
                Write(bms, encoder, fs);
            }
        }

        /// <summary>
        /// Gets the bytes of the bitmap in the given encoding
        /// </summary>
        public static byte[] Write(BitmapSource bms, BitmapEncoder encoder)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                Write(bms, encoder, ms);
                return ms.ToArray();
            }
        }

        /// <summary>
        /// Reads an image from the given filepath
        /// </summary>
        public static BitmapImage Read(string path)
        {
            using (FileStream fs = File.OpenRead(path))
            {
                return Read(fs);
            }
        }

        /// <summary>
        /// Reads an image from the given stream
        /// </summary>
        public static BitmapImage Read(Stream stream)
        {
            BitmapImage bmi = new BitmapImage();
            bmi.BeginInit();
            bmi.CacheOption = BitmapCacheOption.OnLoad;
            bmi.StreamSource = stream;
            bmi.EndInit();
            bmi.Freeze();

            return bmi;
        }

        /// <summary>
        /// Reads an image from the given bytes
        /// </summary>
        public static BitmapImage Read(byte[] bytes)
        {
            using (MemoryStream ms = new MemoryStream(bytes))
            {
                return Read(ms);
            }
        }

        /// <summary>
        /// Creates a copy of the bitmap at the given size
        /// </summary>
        public static BitmapSource Resize(BitmapSource source, int width, int height)
        {
            Rect rect = new Rect(0, 0, width, height);

            DrawingGroup group = new DrawingGroup();
            RenderOptions.SetBitmapScalingMode(group, BitmapScalingMode.HighQuality);
            group.Children.Add(new ImageDrawing(source, rect));

            DrawingVisual drawingVisual = new DrawingVisual();
            using (DrawingContext dc = drawingVisual.RenderOpen())
            {
                dc.DrawDrawing(group);
            }

            RenderTargetBitmap resizedImage = new RenderTargetBitmap(
                width, height, // Resized dimensions
                96, 96, // Default DPI values
                PixelFormats.Default);

            resizedImage.Render(drawingVisual);

            return resizedImage;
        }

        /// <summary>
        /// Creates a copy of the bitmap at the given size, maintains aspect ratio
        /// </summary>
        public static BitmapSource Resize(BitmapSource bms, int length, bool isWidth)
        {
            if (isWidth)
            {
                double scale = length / (double)bms.PixelWidth;
                int newHeight = (int)(bms.PixelHeight * scale);

                return Resize(bms, length, newHeight);
            }
            else
            {
                double scale = length / (double)bms.PixelHeight;
                int newWidth = (int)(bms.PixelWidth * scale);

                return Resize(bms, newWidth, length);
            }
        }

        /// <summary>
        /// Creates a copy of the bitmap with the given format
        /// </summary>
        public static FormatConvertedBitmap ChangeFormat(BitmapSource bms, PixelFormat newFormat)
        {
            FormatConvertedBitmap newBitmap = new FormatConvertedBitmap();
            newBitmap.BeginInit();
            newBitmap.Source = bms;
            newBitmap.DestinationFormat = newFormat;
            newBitmap.EndInit();

            return newBitmap;
        }

        /// <summary>
        /// Gets the raw bytes of the bitmap
        /// </summary>
        public static byte[] BitmapSourceToRawBytes(BitmapSource bms)
        {
            int bytesPerPixel = bms.Format.BitsPerPixel / 8;

            byte[] bytes = new byte[bms.PixelWidth * bms.PixelHeight * bytesPerPixel];
            bms.CopyPixels(bytes, bms.PixelWidth * bytesPerPixel, 0);

            return bytes;
        }

        /// <summary>
        /// Renders the given element as a bitmap
        /// </summary>
        public static RenderTargetBitmap RenderToBitmap(FrameworkElement target)
        {
            int actualWidth = (int)target.ActualWidth;
            int actualHeight = (int)target.ActualHeight;

            RenderTargetBitmap renderBitmap = new RenderTargetBitmap(actualWidth, actualHeight, 96d, 96d, PixelFormats.Default);

            renderBitmap.Render(target);
            return renderBitmap;
        }

        public static byte[] ClipSize(byte[] imageData, int maxWidth, int maxHeight)
        {
            return ClipSize(imageData, maxWidth, maxHeight, new PngBitmapEncoder() { });
        }

        /// <summary>
        /// Create a smaller version of the given image
        /// </summary>
        public static byte[] ClipSize(byte[] imageData, int maxWidth, int maxHeight, BitmapEncoder encoder)
        {
            BitmapSource bms = ImageTools.Read(imageData);

            if (bms.PixelWidth > maxWidth || bms.PixelHeight > maxHeight)
            {
                // if too wide then resize
                if (bms.PixelWidth > maxWidth)
                {
                    bms = ImageTools.Resize(bms, maxWidth, true);
                }

                // if image is to heigh then crop the bottom
                if (bms.PixelHeight > maxHeight)
                {
                    bms = new CroppedBitmap(bms, new Int32Rect(0, 0, bms.PixelWidth, maxHeight));
                }
            }

            return ImageTools.Write(bms, encoder);
        }

        /// <summary>
        /// Makes the filter string for an OpenFileDialog to choose all supported images
        /// </summary>
        public static string MakeImageDlgFilter()
        {
            const string filter = "Image files|*.bmp;*.dib;*.rle;*.jpg;*.jpeg;*.jpe;*.jfif;*.gif;*.tif;*.tiff;*.png|" +
                                  "bmp files (*.bmp;*.dib;*.rle)|*.bmp;*.dib;*.rle|" +
                                  "jpeg files (*.jpg;*.jpeg;*.jpe;*.jfif)|*.jpg;*.jpeg;*.jpe;*.jfif|" +
                                  "gif files (*.gif)|*.gif|" +
                                  "tiff files (*.tif;*.tiff)|*.tif;*.tiff|" +
                                  "png files (*.png)|*.png";
            return filter;
        }

    }
}