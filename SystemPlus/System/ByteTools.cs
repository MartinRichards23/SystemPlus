namespace SystemPlus
{
    /// <summary>
    /// Helpers for dealing with byte[] data
    /// </summary>
    public static class ByteTools
    {
        public static uint SwapEndianness(ulong x)
        {
            return (uint)(((x & 0x000000ff) << 24) + ((x & 0x0000ff00) << 8) + ((x & 0x00ff0000) >> 8) + ((x & 0xff000000) >> 24));
        }

        /// <summary>
        /// Finds a specific byte index within an array of bytes
        /// </summary>
        /// <returns>Returns the array poistion if found or -1 if not</returns>
        static int Find(int start, int end, byte[] data, byte target)
        {
            for (int i = start; (i < data.Length && i <= end); i++)
            {
                if (data[i] == target)
                    return i;
            }

            return -1;
        }

        /// <summary>
        /// Finds index of byte array within another array
        /// </summary>
        /// <returns>Returns the array poistion if found or -1 if not</returns>
        public static int FindSequence(int start, int end, byte[] data, byte[] target)
        {
            // check the parameters are sensible
            if ((start > data.Length) || (start >= end) || (end > data.Length))
                return -1;

            // if they are then proceed
            int pos = start;
            bool found = false;

            do
            {
                // try and find the first byte of needle in the haystack
                pos = Find(pos, end, data, target[0]);
                if (pos > -1)
                {
                    // have found an occurence of the first character inthe array
                    // so will check the follwoing characters to see if the whole message is there
                    found = CompareMessage(pos, data, target);
                }
                pos += 1; // so dont keep looping round on the same result move it on 1
            } while ((pos > 0) & (!found));

            if (found)
                return pos - 1; // return -1 because we added 1 on last line of loop

            return -1;
        }

        public static bool CompareMessage(int startingPoint, byte[] haystack, byte[] needle)
        {
            // will start at a specific point in the haystack and check the subsequent 
            // bytes for the full needle message - if it fails then will return false
            // if find it then return true

            for (int i = 0; i < needle.Length; i++)
            {
                if (haystack[startingPoint + i] != needle[i])
                    return false; // there has been a mismatch
            }
            return true;
        }

    }
}
