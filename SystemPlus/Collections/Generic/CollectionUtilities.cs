using System.Collections.Generic;

namespace SystemPlus.Collections.Generic
{
    /// <summary>
    /// Collection utilities
    /// </summary>
    public static class CollectionUtilities
    {
        /// <summary>
        /// Helper method to decide if two lists are equals
        /// </summary>
        public static bool AreListsEqual<T>(IList<T> list1, IList<T> list2)
        {
            // check for null
            if (list1 == null && list2 == null)
                return true;

            if (list1 == null || list2 == null || list1.Count != list2.Count)
                return false;

            for (int i = 0; i < list1.Count; i++)
            {
                if (!Equals(list1[i], list2[i]))
                    return false;
            }

            return true;
        }
    }
}