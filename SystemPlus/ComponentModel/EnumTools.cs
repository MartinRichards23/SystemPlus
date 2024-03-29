﻿using System.ComponentModel;
using System.Reflection;

namespace SystemPlus.ComponentModel
{
    /// <summary>
    /// Enum tools and extensions
    /// </summary>
    public static class EnumTools
    {
        public static string GetEnumDescription(object enumValue, Type enumType)
        {
            string? enumString = enumValue?.ToString();

            if (enumString == null)
                return string.Empty;

            FieldInfo? field = enumType?.GetField(enumString);

            if (field == null)
                return string.Empty;

            object[] atts = field.GetCustomAttributes(typeof(DescriptionAttribute), false);

            if (atts.FirstOrDefault() is DescriptionAttribute descriptionAttribute)
                return descriptionAttribute.Description;

            return string.Empty;
        }

        public static InfoAttribute? GetEnumInfo(object enumValue, Type enumType)
        {
            string? enumString = enumValue?.ToString();

            if (enumString == null)
                return null;

            return enumType?.GetField(enumString)?.GetCustomAttributes(typeof(InfoAttribute), false).FirstOrDefault() as InfoAttribute;
        }
    }

    /// <summary>
    /// This attribute is used to represent a string value
    /// for a value in an enum.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Enum | AttributeTargets.Interface | AttributeTargets.Delegate)]
    public sealed class InfoAttribute : Attribute, INotifyPropertyChanged
    {
        public string Name { get; }
        public string? Description { get; }

        public event PropertyChangedEventHandler? PropertyChanged;

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