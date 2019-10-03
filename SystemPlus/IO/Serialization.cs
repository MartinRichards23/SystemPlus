using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace SystemPlus.IO
{
    /// <summary>
    /// Helpers for serialization
    /// </summary>
    public static class Serialization
    {
        #region Xml

        public static void XmlSerialize<T>(T obj, Stream data, bool hideDeclaration = true, bool indent = true, bool hideNameSpaces = true, bool checkChars = false)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            XmlSerializerNamespaces xsn = new XmlSerializerNamespaces();
            XmlWriterSettings settings = new XmlWriterSettings
            {
                OmitXmlDeclaration = hideDeclaration,
                Indent = indent,
                CheckCharacters = checkChars
            };

            if (hideNameSpaces)
                xsn.Add(string.Empty, string.Empty);

            using (XmlWriter writer = XmlWriter.Create(data, settings))
            {
                serializer.Serialize(writer, obj, xsn);
            }
        }

        public static string XmlSerialize<T>(T obj, bool hideDeclaration = true, bool indent = true, bool hideNameSpaces = true, bool checkChars = false)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                XmlSerialize(obj, ms, hideDeclaration, indent, hideNameSpaces, checkChars);
                ms.Position = 0;

                using (StreamReader sw = new StreamReader(ms))
                {
                    return sw.ReadToEnd();
                }
            }
        }

        public static void XmlSerialize<T>(T obj, FileInfo file, bool hideDeclaration = true, bool indent = true, bool hideNameSpaces = true, bool checkChars = false)
        {
            if (!file.Directory.Exists)
                file.Directory.Create();

            using (FileStream fs = File.Create(file.FullName))
            {
                XmlSerialize(obj, fs, hideDeclaration, indent, hideNameSpaces, checkChars);
            }
        }

        public static T XmlDeserialize<T>(Stream data)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            XmlReaderSettings settings = new XmlReaderSettings
            {
                CheckCharacters = false
            };

            using (XmlReader reader = XmlReader.Create(data, settings))
            {
                return (T)serializer.Deserialize(reader);
            }
        }

        public static T XmlDeserialize<T>(string data)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            using (StringReader reader = new StringReader(data))
            {
                return (T)serializer.Deserialize(reader);
            }
        }

        public static T XmlDeserialize<T>(FileInfo file)
        {
            using (FileStream fs = File.OpenRead(file.FullName))
            {
                return XmlDeserialize<T>(fs);
            }
        }

        #endregion

        #region DataContract

        public static void DataSerialize<T>(T obj, Stream stream, XmlWriterSettings settings = null, IEnumerable<Type> knownTypes = null)
        {
            DataContractSerializer serializer = new DataContractSerializer(typeof(T), knownTypes);

            using (XmlWriter writer = XmlWriter.Create(stream, settings))
            {
                serializer.WriteObject(writer, obj);
            }
        }

        public static void DataSerialize<T>(T obj, TextWriter output, XmlWriterSettings settings = null, IEnumerable<Type> knownTypes = null)
        {
            DataContractSerializer serializer = new DataContractSerializer(typeof(T), knownTypes);

            using (XmlWriter writer = XmlWriter.Create(output, settings))
            {
                serializer.WriteObject(writer, obj);
            }
        }

        public static void DataSerialize<T>(T obj, FileInfo file, XmlWriterSettings settings = null)
        {
            if (!file.Directory.Exists)
                file.Directory.Create();

            using (FileStream fs = File.Create(file.FullName))
            {
                DataSerialize(obj, fs, settings);
            }
        }

        public static string DataSerialize<T>(T obj, XmlWriterSettings settings = null)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                DataSerialize(obj, ms, settings);
                ms.Position = 0;

                using (StreamReader sw = new StreamReader(ms))
                {
                    return sw.ReadToEnd();
                }
            }
        }

        public static T DataDeserialize<T>(Stream stream, XmlReaderSettings settings = null, IEnumerable<Type> knownTypes = null)
        {
            DataContractSerializer serializer = new DataContractSerializer(typeof(T), knownTypes);

            using (XmlReader reader = XmlReader.Create(stream, settings))
            {
                return (T)serializer.ReadObject(reader);
            }
        }

        public static T DataDeserialize<T>(TextReader input, XmlReaderSettings settings = null, IEnumerable<Type> knownTypes = null)
        {
            DataContractSerializer serializer = new DataContractSerializer(typeof(T), knownTypes);
            
            using (XmlReader reader = XmlReader.Create(input, settings))
            {
                return (T)serializer.ReadObject(reader);
            }
        }

        public static T DataDeserialize<T>(string data, XmlReaderSettings settings = null, IEnumerable<Type> knownTypes = null)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(data);
            using (MemoryStream mem = new MemoryStream(bytes))
            {
                return DataDeserialize<T>(mem, settings, knownTypes);
            }
        }

        public static T DataDeserialize<T>(FileInfo file, XmlReaderSettings settings = null, IEnumerable<Type> knownTypes = null)
        {
            using (FileStream fs = File.OpenRead(file.FullName))
            {
                return DataDeserialize<T>(fs, settings, knownTypes);
            }
        }

        #endregion

        #region Json

        public static void JsonSerialize<T>(T obj, Stream data, IEnumerable<Type> knownTypes = null) where T : class, new()
        {
            DataContractJsonSerializer json = new DataContractJsonSerializer(typeof(T), knownTypes);
            json.WriteObject(data, obj);
        }

        public static string JsonSerialize<T>(T obj, IEnumerable<Type> knownTypes = null) where T : class, new()
        {
            using (MemoryStream ms = new MemoryStream())
            using (StreamReader sr = new StreamReader(ms))
            {
                JsonSerialize(obj, ms, knownTypes);
                ms.Seek(0, SeekOrigin.Begin);
                return sr.ReadToEnd();
            }
        }

        public static T JsonDeserialize<T>(string data) where T : class, new()
        {
            byte[] bytes = Encoding.UTF8.GetBytes(data);

            using (MemoryStream stream = new MemoryStream(bytes))
            {
                return JsonDeserialize<T>(stream);
            }
        }

        public static T JsonDeserialize<T>(Stream data) where T : class, new()
        {
            DataContractJsonSerializer json = new DataContractJsonSerializer(typeof(T));
            return (T)json.ReadObject(data);
        }

        #endregion

        #region Binary

        public static byte[] BinarySerialize<T>(T obj)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                BinarySerialize(obj, ms);
                return ms.ToArray();
            }
        }

        public static void BinarySerialize<T>(T obj, Stream data)
        {
            BinaryFormatter bf = new BinaryFormatter();
            bf.Serialize(data, obj);
        }

        public static void BinarySerialize<T>(T obj, FileInfo file)
        {
            if (!file.Directory.Exists)
                file.Directory.Create();

            using (FileStream fs = File.Create(file.FullName))
                BinarySerialize(obj, fs);
        }

        public static T BinaryDeserialize<T>(byte[] data)
        {
            using (MemoryStream ms = new MemoryStream(data))
                return BinaryDeserialize<T>(ms);
        }

        public static T BinaryDeserialize<T>(Stream data)
        {
            BinaryFormatter bf = new BinaryFormatter();
            return (T)bf.Deserialize(data);
        }

        public static T BinaryDeserialize<T>(FileInfo file)
        {
            using (FileStream fs = File.OpenRead(file.FullName))
                return BinaryDeserialize<T>(fs);
        }

        #endregion
    }
}