﻿using System;
using System.ComponentModel;
using System.Linq;
using System.Windows.Markup;
using SystemPlus.ComponentModel;

namespace SystemPlus.Windows
{
    /// <summary>
    /// Class for helping diplay description of enum in combobox
    /// </summary>
    public class EnumerationExtension : MarkupExtension
    {
        public EnumerationExtension(Type enumType)
        {
            if (enumType == null)
                throw new ArgumentNullException(nameof(enumType));

            enumType = Nullable.GetUnderlyingType(enumType) ?? enumType;

            if (enumType.IsEnum == false)
                throw new ArgumentException("Type must be an Enum.");

            EnumType = enumType;
        }

        public Type EnumType { get; }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return EnumerationMember.GetEnumerationMembers(EnumType);
        }
    }

    public class EnumerationMember : INotifyPropertyChanged
    {
        public string? Description { get; set; }
        public object? Value { get; set; }

        public event PropertyChangedEventHandler? PropertyChanged;

        public static EnumerationMember[] GetEnumerationMembers(Type enumType)
        {
            Array enumValues = Enum.GetValues(enumType);

            return (from object enumValue in enumValues
                    select new EnumerationMember
                    {
                        Value = enumValue,
                        Description = EnumTools.GetEnumDescription(enumValue, enumType)
                    }).ToArray();
        }
    }
}