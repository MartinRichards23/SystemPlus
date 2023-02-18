using System.Collections;
using SystemPlus.Collections.ObjectModel;

namespace SystemPlus.ComponentModel.Ini
{

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

        public IniValue? GetValue(string key)
        {
            string normalKey = IniReader.NormaliseKey(key);
            return values.TryGet(normalKey);
        }

        public IniValue GetOrCreateValue(string key)
        {
            IniValue? iniValue = GetValue(key);

            if (iniValue == null)
            {
                iniValue = new IniValue(key);
                values.Add(iniValue);
            }

            return iniValue;
        }

        public void AddValue(IniValue item)
        {
            values.AddOrReplace(item);
        }

        public void AddValue(string key, object value)
        {
            IniValue iniValue = GetOrCreateValue(key);
            iniValue.Value = value?.ToString();
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
