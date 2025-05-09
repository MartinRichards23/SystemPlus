﻿namespace SystemPlus.Collections.Generic
{
    /// <summary>
    /// Extensions methods for sorting collections
    /// </summary>
    public static class SortExtensions
    {
        /// <summary>
        /// Randomise the order of a collection
        /// </summary>
        public static void Shuffle<T>(this IList<T> list, Random rand)
        {
            ArgumentNullException.ThrowIfNull(list);
            ArgumentNullException.ThrowIfNull(rand);

            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rand.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }

        /// <summary>
        /// Sorts the list based on IComparable<T>
        /// </summary>
        public static void SortItems<T>(this IList<T> list) where T : IComparable<T>
        {
            ArgumentNullException.ThrowIfNull(list);

            for (int i = list.Count - 1; i >= 0; i--)
            {
                for (int j = 1; j <= i; j++)
                {
                    T o1 = list[j - 1];
                    T o2 = list[j];
                    if (o1.CompareTo(o2) > 0)
                    {
                        list.Remove(o1);
                        list.Insert(j, o1);
                    }
                }
            }
        }

        /// <summary>
        /// Sorts the list using a Comparer
        /// </summary>
        public static void SortItems<T>(this IList<T> list, IComparer<T> comparer)
        {
            ArgumentNullException.ThrowIfNull(list);
            ArgumentNullException.ThrowIfNull(comparer);

            for (int i = list.Count - 1; i >= 0; i--)
            {
                for (int j = 1; j <= i; j++)
                {
                    T o1 = list[j - 1];
                    T o2 = list[j];
                    if (comparer.Compare(o1, o2) > 0)
                    {
                        list.Remove(o1);
                        list.Insert(j, o1);
                    }
                }
            }
        }

        /// <summary>
        /// Sorts the list using a function
        /// </summary>
        public static void SortItems<T>(this IList<T> list, Func<T, T, int> compareFunc)
        {
            ArgumentNullException.ThrowIfNull(list);
            ArgumentNullException.ThrowIfNull(compareFunc);

            for (int i = list.Count - 1; i >= 0; i--)
            {
                for (int j = 1; j <= i; j++)
                {
                    T o1 = list[j - 1];
                    T o2 = list[j];
                    if (compareFunc(o1, o2) > 0)
                    {
                        list.Remove(o1);
                        list.Insert(j, o1);
                    }
                }
            }
        }

        public static void InsertSorted<T>(this IList<T> list, T item) where T : IComparable<T>
        {
            ArgumentNullException.ThrowIfNull(list);

            for (int i = 0; i < list.Count; i++)
            {
                T o1 = list[i];
                if (o1.CompareTo(item) > 0)
                {
                    list.Insert(i, item);
                    return;
                }
            }

            list.Add(item);
        }

        public static void InsertSorted<T>(this IList<T> list, T item, IComparer<T> comparer)
        {
            ArgumentNullException.ThrowIfNull(list);
            ArgumentNullException.ThrowIfNull(comparer);

            for (int i = 0; i < list.Count; i++)
            {
                T o1 = list[i];
                if (comparer.Compare(o1, item) > 0)
                {
                    list.Insert(i, item);
                    return;
                }
            }

            list.Add(item);
        }

        public static void InsertSorted<T>(this IList<T> list, T item, Func<T, T, int> compareFunc)
        {
            ArgumentNullException.ThrowIfNull(list);
            ArgumentNullException.ThrowIfNull(compareFunc);

            for (int i = 0; i < list.Count; i++)
            {
                T o1 = list[i];
                if (compareFunc(o1, item) > 0)
                {
                    list.Insert(i, item);
                    return;
                }
            }

            list.Add(item);
        }
    }
}
