using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace SystemPlus
{
    public static class Misc
    {
        /// <summary>
        /// Creates a Comb Guid
        /// </summary>
        public static Guid NewCombGuid()
        {
            byte[] destinationArray = Guid.NewGuid().ToByteArray();
            DateTime time = new DateTime(0x76c, 1, 1);
            DateTime now = DateTime.UtcNow;
            TimeSpan span = new TimeSpan(now.Ticks - time.Ticks);
            TimeSpan timeOfDay = now.TimeOfDay;
            byte[] bytes = BitConverter.GetBytes(span.Days);
            byte[] array = BitConverter.GetBytes((long)(timeOfDay.TotalMilliseconds / 3.333333));
            Array.Reverse(bytes);
            Array.Reverse(array);
            Array.Copy(bytes, bytes.Length - 2, destinationArray, destinationArray.Length - 6, 2);
            Array.Copy(array, array.Length - 4, destinationArray, destinationArray.Length - 4, 4);
            return new Guid(destinationArray);
        }

        /// <summary>
        /// Creates a new Guid by combing 2 existing ones
        /// </summary>
        public static Guid CombineGuids(Guid guid1, Guid guid2)
        {
            const int byteCount = 16;
            byte[] destByte = new byte[byteCount];
            byte[] guid1Byte = guid1.ToByteArray();
            byte[] guid2Byte = guid2.ToByteArray();

            for (int i = 0; i < byteCount; i++)
            {
                destByte[i] = (byte)(guid1Byte[i] ^ guid2Byte[i]);
            }
            return new Guid(destByte);
        }

        public static string NewShortGuid()
        {
            long i = 1;
            foreach (byte b in Guid.NewGuid().ToByteArray())
            {
                i *= (b + 1);
            }
            return $"{i - DateTime.UtcNow.Ticks:x}";
        }

        /// <summary> 
        /// Perform a deep Copy of the object. 
        /// </summary> 
        /// <typeparam name="T">The type of object being copied.</typeparam>
        /// <param name="source">The object instance to copy.</param> 
        /// <returns> The copied object.</returns> 
        public static T DeepClone<T>(T source)
        {
            if (!typeof(T).IsSerializable)
                throw new ArgumentException(@"The type must be serializable.", nameof(source));

            IFormatter formatter = new BinaryFormatter();

            using Stream stream = new MemoryStream();
            formatter.Serialize(stream, source);
            stream.Seek(0, SeekOrigin.Begin);
            return (T)formatter.Deserialize(stream);
        }

        public static IList<KeyValuePair<string, string>> GetCommandLineKeyValues()
        {
            IList<KeyValuePair<string, string>> values = new List<KeyValuePair<string, string>>();
            string[] parameters = Environment.GetCommandLineArgs();

            if (parameters != null)
            {
                foreach (string param in parameters)
                {
                    try
                    {
                        int posEquals = param.IndexOf("=", StringComparison.InvariantCultureIgnoreCase);
                        KeyValuePair<string, string> kvp;

                        if (posEquals > 1)
                        {
                            string key = param.Substring(0, posEquals).ToLower().Trim();
                            string val = param.Substring(posEquals + 1);

                            if (string.IsNullOrWhiteSpace(key))
                                continue;

                            kvp = new KeyValuePair<string, string>(key, val);
                        }
                        else
                        {
                            kvp = new KeyValuePair<string, string>(param, string.Empty);
                        }

                        values.Add(kvp);
                    }
                    catch { }
                }
            }

            return values;
        }
    }

    public static class TConverter
    {
        public static T ChangeType<T>(object value)
        {
            return (T)ChangeType(typeof(T), value);
        }

        public static object ChangeType(Type t, object value)
        {
            TypeConverter tc = TypeDescriptor.GetConverter(t);
            return tc.ConvertFrom(value);
        }

        public static void RegisterTypeConverter<T, TC>() where TC : TypeConverter
        {
            TypeDescriptor.AddAttributes(typeof(T), new TypeConverterAttribute(typeof(TC)));
        }
    }
}