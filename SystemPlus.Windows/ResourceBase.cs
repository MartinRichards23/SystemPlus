using System;
using System.IO;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Windows.Resources;
using SystemPlus.Collections.ObjectModel;

namespace SystemPlus.Windows
{
    public abstract class ResourceBase
    {
        readonly KeyedCollection<IconInfo> Images = new KeyedCollection<IconInfo>();
        readonly object key = new object();

        public abstract BitmapSource GetImage(string name);
        public abstract Stream GetResource(string name);

        /// <summary>
        /// Gets an image from resources, storing it for quick access
        /// </summary>
        public IconInfo GetImageInfo(string path, string name)
        {
            lock (key)
            {
                if (Images.Contains(name))
                {
                    // we have loaded it already
                    return Images[name];
                }
                // load image
                string url = path + name;
                BitmapImage bmi = new BitmapImage(new Uri(url));
                bmi.Freeze();

                IconInfo inf = new IconInfo(name, bmi);

                // store it
                Images.Add(inf);

                return inf;
            }
        }

        /// <summary>
        /// Gets an image from resources, storing it for quick access
        /// </summary>
        public BitmapSource GetImage(string path, string name)
        {
            return GetImageInfo(path, name).Bitmap;
        }

        public Stream GetResource(string path, string name)
        {
            Uri uri = new Uri(path + name);
            StreamResourceInfo sri = Application.GetResourceStream(uri);
            return sri.Stream;
        }
    }

}
