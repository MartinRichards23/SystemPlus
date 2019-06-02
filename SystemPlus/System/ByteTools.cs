namespace SystemPlus
{
    public static class ByteTools
    {
        public static uint SwapEndianness(ulong x)
        {
            return (uint)(((x & 0x000000ff) << 24) + ((x & 0x0000ff00) << 8) + ((x & 0x00ff0000) >> 8) + ((x & 0xff000000) >> 24));
        }

        static int Locate(int startingPoint, int endPosInclusive, byte[] haystack, byte needle)
        {
            // will find a specific byte within an array of bytes
            // returns the array poistion if found or -1 if not
            // the endPoint is there because the buffer may be massive with only a few bytes of interest
            // the endPos will be the last position that is compared

            for (int i = startingPoint; (i < haystack.Length && i <= endPosInclusive); i++)
            {
                if (haystack[i] == needle)
                    return i;
            }

            return -1; // couldnt be found
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

        public static int SearchForByteSequence(int startingPosZeroRefd, int endPosZeroRefd, byte[] haystack, byte[] needle)
        {
            // the endPos is there because the buffer may be massive with only a few bytes of interest

            // check the parameters are sensible
            if ((startingPosZeroRefd > haystack.Length) || (startingPosZeroRefd >= endPosZeroRefd) || (endPosZeroRefd > haystack.Length))
                return -1;

            // if they are then proceed
            int pos = startingPosZeroRefd;
            bool found = false;

            do
            {
                // try and find the first byte of needle in the haystack
                pos = Locate(pos, endPosZeroRefd, haystack, needle[0]);
                if (pos > -1)
                {
                    // have found an occurence of the first character inthe array
                    // so will check the follwoing characters to see if the whole message is there
                    found = CompareMessage(pos, haystack, needle);
                }
                pos += 1; // so dont keep looping round on the same result move it on 1
            } while ((pos > 0) & (!found)); // if>0 because would be -1 if hadnt added 1 on above line
            // the above will keep looping while it can find instances of the 1st byte
            // so long as it doesnt find the following bytes after it
            // once these are found, it will break out
            // will also break out if cant find the first byte in the haystack array

            if (found)
                return pos - 1; // return -1 because we added 1 on last line of loop

            return -1;
        }
    }
}
