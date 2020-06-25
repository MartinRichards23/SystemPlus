using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace SystemPlus.IO
{
    /// <summary>
    /// Extensions methods for streams
    /// </summary>
    public static class StreamExtensions
    {
        public static MemoryStream ToMemoryStream(this Stream stream)
        {
            if (stream == null)
                throw new ArgumentNullException(nameof(stream));

            MemoryStream ms = new MemoryStream();
            stream.CopyTo(ms);
            ms.Position = 0;

            return ms;
        }

        public static async Task<MemoryStream> ToMemoryStreamAsync(this Stream stream)
        {
            if (stream == null)
                throw new ArgumentNullException(nameof(stream));

            MemoryStream ms = new MemoryStream();
            await stream.CopyToAsync(ms);
            ms.Position = 0;

            return ms;
        }

        public static byte[] ToBytes(this Stream stream)
        {
            using (MemoryStream ms = stream.ToMemoryStream())
            {
                return ms.ToArray();
            }
        }

        public static MemoryStream ToStream(this byte[] data)
        {
            return new MemoryStream(data);
        }

        /// <summary>
        /// Gets segment from array
        /// </summary>
        public static T[] SubArray<T>(this T[] data, int index, int length)
        {
            T[] result = new T[length];
            Array.Copy(data, index, result, 0, length);
            return result;
        }

        /// <summary>
        /// Reads the bytes from the current stream and writes them to the destination
        /// </summary>
        public static void CopyTo(this Stream source, Stream destination, int bufferSize, int maxLength)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (destination == null)
                throw new ArgumentNullException(nameof(destination));

            byte[] buffer = new byte[bufferSize];

            int read;
            int totalRead = 0;
            do
            {
                read = source.Read(buffer, 0, buffer.Length);
                totalRead += read;

                destination.Write(buffer, 0, read);

                if (totalRead > maxLength)
                    throw new IOException("Stream exceeded max length");
            } while (read > 0);
        }

        /// <summary>
        /// Writes whole buffer to the stream from the current position
        /// </summary>
        public static void Write(this Stream stream, byte[] buffer)
        {
            if (stream == null)
                throw new ArgumentNullException(nameof(stream));
            if (buffer == null)
                throw new ArgumentNullException(nameof(buffer));

            stream.Write(buffer, 0, buffer.Length);
        }

        public static string Read(this StreamReader reader, int count)
        {
            if (reader == null)
                throw new ArgumentNullException(nameof(reader));

            char[] buffer = new char[count];
            reader.Read(buffer, 0, buffer.Length);
            return new string(buffer);
        }

        public static IEnumerable<string> EnumerateLines(this TextReader reader)
        {
            if (reader == null)
                throw new ArgumentNullException(nameof(reader));

            string? line;
            while ((line = reader.ReadLine()) != null)
            {
                yield return line;
            }
        }
    }
}