using SystemPlus.Collections.ObjectModel;

namespace SystemPlus.ComponentModel.Ini
{
    /// <summary>
    /// Represents a key value pair from an ini file
    /// </summary>
    public class IniValue : IKeyed
    {
        public IniValue(string name)
        {
            Name = name;
            Key = IniReader.NormaliseKey(Name);
        }

        public string Name { get; }
        public string Key { get; }
        public string Value { get; set; }

        public override string ToString()
        {
            return string.Format("{0}, {1}", Name, Value);
        }
    }
}
