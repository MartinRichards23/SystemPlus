using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Media.Imaging;
using SystemPlus.Collections.ObjectModel;

namespace SystemPlus.Windows
{
    public class IconInfo : IKeyed
    {
        public IconInfo(string name, BitmapSource bms, bool rotatable = false)
        {
            Name = name;
            FriendlyName = Path.GetFileNameWithoutExtension(Uri.UnescapeDataString(name));
            Bitmap = bms;
            Rotatable = rotatable;
        }

        public string Name { get; }
        public string FriendlyName { get; }
        public BitmapSource Bitmap { get; }
        public bool Rotatable { get; }

        public string Key
        {
            get { return Name; }
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
