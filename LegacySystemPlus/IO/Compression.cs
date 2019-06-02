using System.IO;
using System.IO.Compression;

namespace SystemPlus.IO
{
    public enum CompressionType { GZip, Deflate }

    public static class Compression
    {
        /// <summary>
        /// GZip compresses the given buffer
        /// </summary>
        public static MemoryStream CompressToStream(byte[] data, CompressionType compression = CompressionType.GZip)
        {
            MemoryStream baseStream = new MemoryStream();

            if (compression == CompressionType.GZip)
            {
                using (Stream stream = new GZipStream(baseStream, CompressionMode.Compress, true))
                {
                    stream.Write(data, 0, data.Length);
                }
            }
            else
            {
                using (Stream stream = new DeflateStream(baseStream, CompressionMode.Compress, true))
                {
                    stream.Write(data, 0, data.Length);
                }
            }

            // only safe to read after deflate closed
            return baseStream;
        }

        /// <summary>
        /// GZip compresses the given buffer
        /// </summary>
        public static byte[] Compress(byte[] data, CompressionType compression = CompressionType.GZip)
        {
            using (MemoryStream ms = CompressToStream(data, compression))
            {
                // only safe to read after deflate closed
                return ms.ToArray();
            }
        }

        /// <summary>
        /// GZip decompresses the given buffer
        /// </summary>
        public static byte[] Decompress(MemoryStream stream, CompressionType compression = CompressionType.GZip)
        {
            if (compression == CompressionType.GZip)
            {
                using (Stream zip = new GZipStream(stream, CompressionMode.Decompress))
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    zip.CopyTo(memoryStream);
                    return memoryStream.ToArray();
                }
            }
            else
            {
                using (Stream zip = new DeflateStream(stream, CompressionMode.Decompress))
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    zip.CopyTo(memoryStream);
                    return memoryStream.ToArray();
                }
            }

        }

        /// <summary>
        /// GZip decompresses the given buffer
        /// </summary>
        public static byte[] Decompress(byte[] data, CompressionType compression = CompressionType.GZip)
        {
            using (MemoryStream stream = new MemoryStream(data))
            {
                return Decompress(stream, compression);
            }
        }

        public static bool CheckMagicNumberGZip(byte[] buffer)
        {
            // gzip pattern
            byte[] pattern = { 31, 139, 8 };

            return CheckMagicNumber(buffer, pattern);
        }

        /// <summary>
        /// Checks buffer magic number matches pattern
        /// </summary>
        public static bool CheckMagicNumber(byte[] buffer, byte[] pattern)
        {
            if (buffer.Length < pattern.Length)
                return false;

            for (int i = 0; i < pattern.Length; i++)
            {
                byte b = buffer[i];
                byte p = pattern[i];

                if (b != p)
                    return false;
            }

            return true;
        }
    }
}
