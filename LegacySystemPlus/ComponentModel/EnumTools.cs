using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace SystemPlus.ComponentModel
{
    public static class EnumTools
    {
        public static string GetEnumDescription(object enumValue, Type enumType)
        {
            FieldInfo field = enumType.GetField(enumValue.ToString());

            if (field == null)
                return enumValue.ToString();

            object[] atts = field.GetCustomAttributes(typeof(DescriptionAttribute), false);

            DescriptionAttribute descriptionAttribute = atts.FirstOrDefault() as DescriptionAttribute;

            if (descriptionAttribute == null)
                return enumValue.ToString();

            return descriptionAttribute.Description;
        }

        public static InfoAttribute GetEnumInfo(object enumValue, Type enumType)
        {
            InfoAttribute descriptionAttribute = enumType.GetField(enumValue.ToString()).GetCustomAttributes(typeof(InfoAttribute), false).FirstOrDefault() as InfoAttribute;

            return descriptionAttribute;
        }
    }

    /// <summary>
    /// This attribute is used to represent a string value
    /// for a value in an enum.
    /// </summary>
    public class InfoAttribute : Attribute, INotifyPropertyChanged
    {
        public string Name { get; protected set; }
        public string Description { get; protected set; }

#pragma warning disable CS0067 // The event 'InfoAttribute.PropertyChanged' is never used
        public event PropertyChangedEventHandler PropertyChanged;
#pragma warning restore CS0067 // The event 'InfoAttribute.PropertyChanged' is never used

        public InfoAttribute(string name)
        {
            Name = name;
            Description = null;
        }

        public InfoAttribute(string name, string description)
        {
            Name = name;
            Description = description;
        }

        public override string ToString()
        {
            string s = Name;

            if (!string.IsNullOrWhiteSpace(Description))
                s += ": " + Description;

            return s;
        }
    }
}