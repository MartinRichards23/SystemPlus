using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using SystemPlus.Collections.ObjectModel;
using SystemPlus.IO;
using SystemPlus.Text;

namespace SystemPlus.ComponentModel.Ini
{
    /// <summary>
    /// Represents an ini file
    /// </summary>
    public class IniReader
    {
        readonly KeyedCollection<IniSection> sections = new KeyedCollection<IniSection>();

        public IniReader()
        {

        }

        public bool HasKey(string sectionName, string key)
        {
            IniSection section = GetSection(sectionName);

            if (section == null)
                return false;

            return section.HasKey(key);
        }

        #region Get / Set values

        public IniSection GetSection(string sectionName)
        {
            string normalSectionName = NormaliseKey(sectionName);
            return sections.TryGet(normalSectionName);
        }

        public IniSection GetOrCreateSection(string sectionName)
        {
            IniSection section = GetSection(sectionName);
            if (section == null)
            {
                section = new IniSection(sectionName);
                sections.Add(section);
            }

            return section;
        }

        public IEnumerable<IniValue> GetSectionValues(string sectionName)
        {
            IniSection section = GetSection(sectionName);

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

            IniSection section = sections.TryGet(normalSectionName);
            return section?.GetValue(normalKey);
        }

        public void SetValue(string sectionName, string key, bool value)
        {
            string val = value ? "1" : "0";

            IniSection section = GetOrCreateSection(sectionName);
            section.AddValue(key, val);
        }

        public void SetValue(string sectionName, string key, string value)
        {
            IniSection section = GetOrCreateSection(sectionName);
            section.AddValue(key, value);
        }

        public void SetValue(string sectionName, string key, int value)
        {
            IniSection section = GetOrCreateSection(sectionName);
            section.AddValue(key, value.ToString(CultureInfo.InvariantCulture));
        }

        public void SetValue(string sectionName, string key, long value)
        {
            IniSection section = GetOrCreateSection(sectionName);
            section.AddValue(key, value.ToString(CultureInfo.InvariantCulture));
        }

        public void SetValue(string sectionName, string key, double value)
        {
            IniSection section = GetOrCreateSection(sectionName);
            section.AddValue(key, value.ToString(CultureInfo.InvariantCulture));
        }

        public void SetJson<T>(string sectionName, string key, T value) where T : class, new()
        {
            string json;

            if (value != null)
                json = Serialization.JsonSerialize(value);
            else
                json = string.Empty;

            IniSection section = GetOrCreateSection(sectionName);
            section.AddValue(key, json);
        }

        public bool GetString(string sectionName, string key, ref string value)
        {
            IniValue? iniValue = GetIniValue(sectionName, key);
            if (iniValue == null)
                return false;

            value = iniValue.Value;
            return true;
        }

        public string GetString(string sectionName, string key, string defaultValue)
        {
            string value = string.Empty;

            if (GetString(sectionName, key, ref value))
                return value;

            return defaultValue;
        }

        public bool GetBool(string sectionName, string key, ref bool value)
        {
            IniValue? iniValue = GetIniValue(sectionName, key);
            if (iniValue == null)
                return false;

            value = ParseBool(iniValue.Value);
            return true;
        }

        public bool GetBool(string sectionName, string key, bool defaultValue)
        {
            bool value = false;
            if (GetBool(sectionName, key, ref value))
                return value;

            return defaultValue;
        }

        public bool? GetBool(string sectionName, string key, bool? defaultValue)
        {
            bool value = false;
            if (GetBool(sectionName, key, ref value))
                return value;

            return defaultValue;
        }

        public bool GetInt(string sectionName, string key, ref int value)
        {
            IniValue? iniValue = GetIniValue(sectionName, key);
            if (iniValue == null)
                return false;

            if (int.TryParse(iniValue.Value, out int val))
            {
                value = val;
                return true;
            }

            return false;
        }

        public int GetInt(string sectionName, string key, int defaultValue)
        {
            int value = 0;
            if (GetInt(sectionName, key, ref value))
                return value;

            return defaultValue;
        }

        public bool GetLong(string sectionName, string key, ref long value)
        {
            IniValue? iniValue = GetIniValue(sectionName, key);
            if (iniValue == null)
                return false;

            if (long.TryParse(iniValue.Value, out long val))
            {
                value = val;
                return true;
            }

            return false;
        }

        public long GetLong(string sectionName, string key, long defaultValue)
        {
            long value = 0;
            if (GetLong(sectionName, key, ref value))
                return value;

            return defaultValue;
        }

        public bool GetDouble(string sectionName, string key, ref double value)
        {
            IniValue? iniValue = GetIniValue(sectionName, key);
            if (iniValue == null)
                return false;

            if (double.TryParse(iniValue.Value, out double val))
            {
                value = val;
                return true;
            }

            return false;
        }

        public double GetDouble(string sectionName, string key, double defaultValue)
        {
            double value = 0;
            if (GetDouble(sectionName, key, ref value))
                return value;

            return defaultValue;
        }

        public T GetJson<T>(string sectionName, string key, T defValue) where T : class, new()
        {
            try
            {
                string result = GetString(sectionName, key, string.Empty);

                if (string.IsNullOrWhiteSpace(result))
                    return defValue;

                return Serialization.JsonDeserialize<T>(result);
            }
            catch
            {
                return defValue;
            }
        }

        public T GetEnum<T>(string sectionName, string key, T defValue) where T : struct
        {
            string val = GetString(sectionName, key, string.Empty);

            if (Enum.TryParse(val, true, out T result))
                return result;

            return defValue;
        }

        #endregion

        #region Load / Save methods

        public void Clear()
        {
            sections.Clear();
        }

        public void LoadFile(string filePath)
        {
            if (!File.Exists(filePath))
                return;

            using (FileStream fs = File.OpenRead(filePath))
            using (StreamReader sr = new StreamReader(fs))
            {
                Load(sr);
            }
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
