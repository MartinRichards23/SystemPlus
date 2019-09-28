using System.Collections;
using System.Collections.Generic;
using SystemPlus.Collections.ObjectModel;

namespace SystemPlus.ComponentModel.Ini
{
    /// <summary>
    /// Represents a section from an ini file
    /// </summary>
    public class IniSection : IKeyed, IEnumerable<IniValue>
    {
        public string Key { get; }
        public string Name { get; }
        readonly KeyedCollection<IniValue> values = new KeyedCollection<IniValue>();

        public IniSection(string name)
        {
            Name = name;
            Key = IniReader.NormaliseKey(Name);
        }

        public bool HasKey(string key)
        {
            string normalKey = IniReader.NormaliseKey(key);
            return values.Contains(normalKey);
        }

        public IniValue GetValue(string key)
        {
            string normalKey = IniReader.NormaliseKey(key);
            return values.TryGet(normalKey);
        }

        public void AddValue(IniValue item)
        {
            values.AddOrReplace(item);
        }

        public void AddValue(string key, object value)
        {
            IniValue iniValue = GetValue(key);

            if (iniValue != null)
            {
                iniValue.Value = value?.ToString();
            }
            else
            {
                iniValue = new IniValue(key)
                {
                    Value = value?.ToString(),
                };

                values.Add(iniValue);
            }
        }

        public override string ToString()
        {
            return Name;
        }

        public IEnumerator<IniValue> GetEnumerator()
        {
            return values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return values.GetEnumerator();
        }
    }
}
