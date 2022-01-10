namespace SystemPlus.Collections
{
    /// <summary>
    /// Array extension methods
    /// </summary>
    public static class ArrayExtensions
    {
        public static T GetValue<T>(this T[] array, int index)
        {
            return array[index];
        }

        public static T GetValue<T>(this T[] array, int index, T defaultVal)
        {
            if (index >= array.Length)
                return defaultVal;

            return array[index];
        }
    }
}
