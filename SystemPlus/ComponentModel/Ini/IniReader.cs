using System.Collections;
using System.IO;
using SystemPlus.Collections.ObjectModel;
using SystemPlus.IO;
using SystemPlus.Text;

namespace SystemPlus.ComponentModel.Ini
{
    public class IniReader : IEnumerable<IniSection>
    {
        readonly KeyedCollection<IniSection> sections = new KeyedCollection<IniSection>();

        public IniReader()
        {

        }

        public bool HasKey(string sectionName, string key)
        {
            IniSection? section = GetSection(sectionName);

            if (section == null)
                return false;

            return section.HasKey(key);
        }

        #region Get / Set values

        public IniSection? GetSection(string sectionName)
        {
            string normalSectionName = NormaliseKey(sectionName);
            return sections.TryGet(normalSectionName);
        }

        public IniSection GetOrCreateSection(string sectionName)
        {
            IniSection? section = GetSection(sectionName);

            if (section == null)
            {
                section = new IniSection(sectionName);
                sections.Add(section);
            }

            return section;
        }

        public IEnumerable<IniValue> GetSectionValues(string sectionName)
        {
            IniSection? section = GetSection(sectionName);

            if (section != null)
            {
                foreach (IniValue value in section)
                {
                    yield return value;
                }
            }
        }

        public IniValue? GetIniValue(string sectionName, string key)
        {
            string normalSectionName = NormaliseKey(sectionName);
            string normalKey = NormaliseKey(key);

            IniSection? section = sections.TryGet(normalSectionName);
            return section?.GetValue(normalKey);
        }

        public void SetValue(string sectionName, string key, string value)
        {
            IniSection section = GetOrCreateSection(sectionName);
            IniValue iniValue = section.GetOrCreateValue(key);
            iniValue.SetValue(value);
        }

        public void SetValue(string sectionName, string key, bool value)
        {
            IniSection section = GetOrCreateSection(sectionName);
            IniValue iniValue = section.GetOrCreateValue(key);
            iniValue.SetValue(value);
        }

        public void SetValue(string sectionName, string key, int value)
        {
            IniSection section = GetOrCreateSection(sectionName);
            IniValue iniValue = section.GetOrCreateValue(key);
            iniValue.SetValue(value);
        }

        public void SetValue(string sectionName, string key, long value)
        {
            IniSection section = GetOrCreateSection(sectionName);
            IniValue iniValue = section.GetOrCreateValue(key);
            iniValue.SetValue(value);
        }

        public void SetValue(string sectionName, string key, double value)
        {
            IniSection section = GetOrCreateSection(sectionName);
            IniValue iniValue = section.GetOrCreateValue(key);
            iniValue.SetValue(value);
        }

        public void SetValue(string sectionName, string key, double? value)
        {
            IniSection section = GetOrCreateSection(sectionName);
            IniValue iniValue = section.GetOrCreateValue(key);
            iniValue.SetValue(value);
        }

        public void SetJson<T>(string sectionName, string key, T value) where T : class, new()
        {
            IniSection section = GetOrCreateSection(sectionName);
            IniValue iniValue = section.GetOrCreateValue(key);
            iniValue.SetJson(value);
        }

        public void SetValue(string sectionName, string key, Enum value)
        {
            IniSection section = GetOrCreateSection(sectionName);
            IniValue iniValue = section.GetOrCreateValue(key);
            iniValue.SetValue(value);
        }

        public string GetString(string sectionName, string key, string defaultValue)
        {
            IniValue? iniValue = GetIniValue(sectionName, key);
            if (iniValue == null)
                return defaultValue;

            return iniValue.GetString(defaultValue);
        }

        public bool GetBool(string sectionName, string key, bool defaultValue)
        {
            IniValue? iniValue = GetIniValue(sectionName, key);
            if (iniValue == null)
                return defaultValue;

            return iniValue.GetBool(defaultValue);
        }

        public bool? GetBool(string sectionName, string key, bool? defaultValue)
        {
            IniValue? iniValue = GetIniValue(sectionName, key);
            if (iniValue == null)
                return defaultValue;

            return iniValue.GetBool(defaultValue);
        }

        public int GetInt(string sectionName, string key, int defaultValue)
        {
            IniValue? iniValue = GetIniValue(sectionName, key);
            if (iniValue == null)
                return defaultValue;

            return iniValue.GetInt(defaultValue);
        }

        public long GetLong(string sectionName, string key, long defaultValue)
        {
            IniValue? iniValue = GetIniValue(sectionName, key);
            if (iniValue == null)
                return defaultValue;

            return iniValue.GetLong(defaultValue);
        }

        public double GetDouble(string sectionName, string key, double defaultValue)
        {
            IniValue? iniValue = GetIniValue(sectionName, key);
            if (iniValue == null)
                return default;

            return iniValue.GetDouble(defaultValue);
        }

        public T GetJson<T>(string sectionName, string key, T defaultValue) where T : class, new()
        {
            IniValue? iniValue = GetIniValue(sectionName, key);
            if (iniValue == null)
                return defaultValue;

            return iniValue.GetJson(defaultValue);
        }

        public T GetEnum<T>(string sectionName, string key, T defaultValue) where T : struct
        {
            IniValue? iniValue = GetIniValue(sectionName, key);
            if (iniValue == null)
                return defaultValue;

            return iniValue.GetEnum(defaultValue);
        }

        #endregion

        #region Load / Save methods

        public void Clear()
        {
            sections.Clear();
        }

        public bool LoadFile(string filePath)
        {
            if (!File.Exists(filePath))
                return false;

            using (FileStream fs = File.OpenRead(filePath))
            using (StreamReader sr = new StreamReader(fs))
            {
                Load(sr);
            }

            return true;
        }

        public void Load(string data)
        {
            using (StringReader sr = new StringReader(data))
            {
                Load(sr);
            }
        }

        public void Load(Stream data)
        {
            using (StreamReader sr = new StreamReader(data))
            {
                Load(sr);
            }
        }

        public void Load(TextReader reader)
        {
            IniSection? currentSection = null;

            foreach (string line in reader.EnumerateLines())
            {
                if (string.IsNullOrEmpty(line))
                    continue; // blank
                if (line.StartsWith(";", StringComparison.Ordinal))
                    continue; // comment
                if (line.StartsWith("[", StringComparison.Ordinal))
                {
                    // section line
                    string name = line.GetFragment("[", "]");
                    currentSection = GetOrCreateSection(name);
                    continue;
                }
                if (currentSection == null)
                    continue;
                if (!line.Contains("="))
                {
                    // no "=", therefore not valid key=value line
                    continue;
                }

                string key = line.GetFragment(null, "=").Trim();
                string value = line.GetFragment("=").Trim();

                IniValue val = new IniValue(key) { Value = value };

                currentSection.AddValue(val);
            }
        }

        public void Save(string path)
        {
            using (FileStream fs = File.Create(path))
            using (TextWriter sw = new StreamWriter(fs))
            {
                Save(sw);
            }
        }

        public void Save(TextWriter sw)
        {
            foreach (IniSection section in sections)
            {
                sw.WriteLine("[{0}]", section.Name);

                foreach (IniValue iniValue in section)
                {
                    sw.WriteLine("{0}={1}", iniValue.Name, iniValue.Value);
                }

                sw.WriteLine();
            }
        }

        public string Save()
        {
            using (StringWriter sw = new StringWriter())
            {
                foreach (IniSection section in sections)
                {
                    sw.WriteLine("[{0}]", section.Name);

                    foreach (IniValue iniValue in section)
                    {
                        sw.WriteLine("{0}={1}", iniValue.Name, iniValue.Value);
                    }

                    sw.WriteLine();
                }

                return sw.ToString();
            }
        }

        #endregion

        public IEnumerator<IniSection> GetEnumerator()
        {
            foreach (IniSection section in sections)
            {
                yield return section;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #region Statics

        public static bool ParseBool(string? value, bool defaultVal = false)
        {
            if (bool.TryParse(value, out bool b))
                return b;
            if (value == "1")
                return true;
            if (value == "0")
                return false;

            return defaultVal;
        }

        public static string NormaliseKey(string key)
        {
            return key.ToUpperInvariant().Trim();
        }

        #endregion
    }

}
