using System.Globalization;
using System.Text.Json;
using SystemPlus.Collections.ObjectModel;

namespace SystemPlus.ComponentModel.Ini
{
    public class IniValue : IKeyed
    {
        public IniValue(string name)
        {
            Name = name;
            Key = IniReader.NormaliseKey(Name);
        }

        public string Name { get; }
        public string Key { get; }
        public string? Value { get; set; }

        public void SetValue(string? value)
        {
            Value = value;
        }

        public void SetValue(bool value)
        {
            Value = value ? "1" : "0";
        }

        public void SetValue(int value)
        {
            Value = value.ToString(CultureInfo.InvariantCulture);
        }

        public void SetValue(long value)
        {
            Value = value.ToString(CultureInfo.InvariantCulture);
        }

        public void SetValue(double value)
        {
            Value = value.ToString(CultureInfo.InvariantCulture);
        }

        public void SetValue(double? value)
        {
            Value = value?.ToString(CultureInfo.InvariantCulture);
        }

        public void SetJson<T>(T value) where T : class, new()
        {
            string? json;

            if (value != null)
                json = JsonSerializer.Serialize(value);
            else
                json = null;

            Value = json;
        }

        public void SetValue(Enum value)
        {
            Value = value.ToString();
        }

        public string GetString(string defaultValue)
        {
            return Value ?? defaultValue;
        }

        public bool GetBool(bool defaultValue)
        {
            return IniReader.ParseBool(Value, defaultValue);
        }

        public bool? GetBool(bool? defaultValue)
        {
            return IniReader.ParseBool(Value, defaultValue ?? false);
        }

        public int GetInt(int defaultValue)
        {
            if (int.TryParse(Value, out int val))
                return val;

            return defaultValue;
        }

        public long GetLong(long defaultValue)
        {
            if (long.TryParse(Value, out long val))
                return val;

            return defaultValue;
        }

        public double GetDouble(double defaultValue)
        {
            if (double.TryParse(Value, out double val))
                return val;

            return defaultValue;
        }

        public T GetJson<T>(T defaultValue) where T : class, new()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(Value))
                    return defaultValue;

                return JsonSerializer.Deserialize<T>(Value) ?? defaultValue;
            }
            catch
            {
                return defaultValue;
            }
        }

        public T GetEnum<T>(T defaultValue) where T : struct
        {
            if (Enum.TryParse(Value, true, out T result))
                return result;

            return defaultValue;
        }

        public override string ToString()
        {
            return string.Format("{0}, {1}", Name, Value);
        }
    }
}
