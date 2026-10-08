using System.Buffers.Binary;

namespace SystemPlus
{
    /// <summary>
    /// Helpers for dealing with byte[] data
    /// </summary>
    public static class ByteTools
    {
        /// <summary>
        /// Finds index
        /// </summary>
        /// <param name="start">Inclusive start index</param>
        /// <param name="end">Inclusive end index, the whole match must lie within start..end</param>
        /// <returns>Returns the array poistion if found or -1 if not</returns>
        public static int FindSequence(int start, int end, byte[] data, byte[] target)
        {
            if (data == null)
                throw new ArgumentNullException(nameof(data));
            if (target == null)
                throw new ArgumentNullException(nameof(target));
            if (target.Length == 0)
                throw new ArgumentException("Target must not be empty", nameof(target));

            // check the parameters are sensible
            if (start < 0 || start > end || start >= data.Length)
                return -1;

            int length = Math.Min(end, data.Length - 1) - start + 1;
            int index = data.AsSpan(start, length).IndexOf(target);

            return index < 0 ? -1 : start + index;
        }

        public static bool CompareMessage(int startingPoint, byte[] haystack, byte[] needle)
        {
            if (haystack == null)
                throw new ArgumentNullException(nameof(haystack));
            if (needle == null)
                throw new ArgumentNullException(nameof(needle));

            // will start at a specific point in the haystack and check the subsequent 
            // bytes for the full needle message - if it fails then will return false
            // if find it then return true

            if (startingPoint < 0 || startingPoint + needle.Length > haystack.Length)
                return false;

            for (int i = 0; i < needle.Length; i++)
            {
                if (haystack[startingPoint + i] != needle[i])
                    return false; // there has been a mismatch
            }
            return true;
        }

    }
}
