using System;
using System.Linq;

namespace SystemPlus.IO
{
    public static class ByteTools
    {        
        public static long IndexOf(this byte[] src, byte[] pattern)
        {
            int c = src.Length - pattern.Length + 1;
            int j;

            for (long i = 0; i < c; i++)
            {
                if (src[i] != pattern[0])
                    continue;

                for (j = pattern.Length - 1; j >= 1 && src[i + j] == pattern[j]; j--) ;

                if (j == 0)
                    return i;
            }

            return -1;
        }

        /// <summary>
        /// Combines 2 arrays
        /// </summary>
        public static byte[] Combine(byte[] a1, byte[] a2)
        {
            byte[] rv = new byte[a1.Length + a2.Length];
            Buffer.BlockCopy(a1, 0, rv, 0, a1.Length);
            Buffer.BlockCopy(a2, 0, rv, a1.Length, a2.Length);

            return rv;
        }

        /// <summary>
        /// Combines any number of arrays 
        /// </summary>
        public static byte[] Combine(params byte[][] arrays)
        {
            byte[] rv = new byte[arrays.Sum(a => a.Length)];
            int offset = 0;
            foreach (byte[] array in arrays)
            {
                Buffer.BlockCopy(array, 0, rv, offset, array.Length);
                offset += array.Length;
            }
            return rv;
        }
    }
}