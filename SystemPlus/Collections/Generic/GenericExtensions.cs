﻿using System.Collections;
using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;

namespace SystemPlus.Collections.Generic
{
    /// <summary>
    /// Extension methods for collections
    /// </summary>
    public static class GenericExtensions
    {
        /// <summary>
        /// Randomly pick one item in the array.
        /// </summary>
        public static T Random<T>(this IList<T> items)
        {
            return items.Random(new Random());
        }

        /// <summary>
        /// Randomly pick one item in the array.
        /// </summary>
        public static T Random<T>(this IList<T> items, Random rand)
        {
            if (items == null)
                throw new ArgumentNullException(nameof(items));
            if (rand == null)
                throw new ArgumentNullException(nameof(rand));

            int i = rand.Next(items.Count - 1);

            return items[i];
        }

        /// <summary>
        /// Performs the specified action on each item in the collection
        /// </summary>
        public static void ForEach<T>(this IEnumerable<T> enumeration, Action<T> action)
        {
            if (enumeration == null)
                throw new ArgumentNullException(nameof(enumeration));
            if (action == null)
                throw new ArgumentNullException(nameof(action));

            foreach (T item in enumeration)
            {
                action(item);
            }
        }

        /// <summary>
        /// Determine if collection is null or empty
        /// </summary>        
        public static bool IsNullOrEmpty<T>([NotNullWhen(false)] this IEnumerable<T> list)
        {
            return list == null || !list.Any();
        }

        /// <summary>
        /// Clones a generic list.
        /// </summary>
        public static IList<T> Clone<T>(this IList<T> listToClone) where T : ICloneable<T>
        {
            return listToClone.Select(item => item.Clone()).ToList();
        }

        /// <summary>
        /// Removes last items until collection is correct size
        /// </summary>
        public static void LimitSize(this IList list, int maxSize)
        {
            if (list == null)
                throw new ArgumentNullException(nameof(list));

            while (list.Count > maxSize)
            {
                list.RemoveAt(list.Count - 1);
            }
        }

        public static void LimitSizeRandom<T>(this IList<T> list, int maxSize)
        {
            if (list == null)
                throw new ArgumentNullException(nameof(list));

            Random rand = new Random(1);

            while (list.Count > maxSize)
            {
                // choose random index, not the first or last though
                int i = rand.Next(1, list.Count - 2);

                list.RemoveAt(i);
            }
        }

        /// <summary>
        /// Removes and returns first item
        /// </summary>
        public static T TakeFirst<T>(this IList<T> list)
        {
            return list.TakeAt(0);
        }

        /// <summary>
        /// Removes and returns last item
        /// </summary>
        public static T TakeLast<T>(this IList<T> list)
        {
            if (list == null)
                throw new ArgumentNullException(nameof(list));

            return list.TakeAt(list.Count - 1);
        }

        [return: MaybeNull]
        public static T TakeLastOrDefault<T>(this IList<T> list)
        {
            if (list == null)
                throw new ArgumentNullException(nameof(list));

            if (list.Count > 0)
                return list.TakeLast();

            return default;
        }

        /// <summary>
        /// Removes and returns item at given index
        /// </summary>
        public static T TakeAt<T>(this IList<T> list, int index)
        {
            if (list == null)
                throw new ArgumentNullException(nameof(list));

            T item = list[index];
            list.RemoveAt(index);
            return item;
        }

        /// <summary>
        /// Add range to the collection
        /// </summary>
        public static void AddRange<T>(this ICollection<T> list, IEnumerable<T> source)
        {
            if (list == null)
                throw new ArgumentNullException(nameof(list));
            if (source == null)
                return;

            foreach (T item in source)
            {
                list.Add(item);
            }
        }

        /// <summary>
        /// Add range to the collection if not already exists
        /// </summary>
        public static void TryAddRange<T>(this ICollection<T> list, IEnumerable source)
        {
            if (list == null)
                throw new ArgumentNullException(nameof(list));
            if (source == null)
                return;

            foreach (T item in source.OfType<T>())
            {
                if (!list.Contains(item))
                    list.Add(item);
            }
        }

        /// <summary>
        /// Add range to the collection
        /// </summary>
        public static void AddRange(this IList list, IEnumerable source)
        {
            if (list == null)
                throw new ArgumentNullException(nameof(list));
            if (source == null)
                return;

            foreach (object? item in source)
            {
                list.Add(item);
            }
        }

        public static T GetValue<T>(this IList<T> array, int index, T defaultVal)
        {
            if (index < 0 || index >= array.Count)
                return defaultVal;

            return array[index];
        }

        /// <summary>
        /// Removes a collection of items from the list
        /// </summary>
        public static void RemoveRange<T>(this ICollection<T> list, IEnumerable<T> source)
        {
            if (list == null)
                throw new ArgumentNullException(nameof(list));
            if (source == null)
                return;

            foreach (T item in source.ToArray())
            {
                list.Remove(item);
            }
        }

        /// <summary>
        /// Removes items from list based on predicate
        /// </summary>
        public static void RemoveWhere<T>(this IList<T> list, Func<T, bool> predicate)
        {
            if (list == null)
                throw new ArgumentNullException(nameof(list));
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

            // iterate list backwards
            for (int i = list.Count - 1; i >= 0; i--)
            {
                T item = list[i];

                // test item against predicate
                if (predicate(item))
                    list.RemoveAt(i);
            }
        }

        public static void RemoveLast<T>(this IList<T> list)
        {
            if (list == null)
                throw new ArgumentNullException(nameof(list));

            list.RemoveAt(list.Count - 1);
        }

        /// <summary>
        /// Iterates an IList in reverse
        /// </summary>
        public static IEnumerable<T> ReverseEnumerate<T>(this IList<T> list)
        {
            if (list == null)
                throw new ArgumentNullException(nameof(list));

            for (int i = list.Count - 1; i >= 0; i--)
            {
                T item = list[i];
                yield return item;
            }
        }

        /// <summary>
        /// Iterates an IList in reverse, performing an action on that item
        /// </summary>
        public static void ReverseEnumerate<T>(this IList<T> list, Action<T> action)
        {
            if (action == null)
                throw new ArgumentNullException(nameof(action));

            foreach (T t in list.ReverseEnumerate())
            {
                action(t);
            }
        }

        /// <summary>
        /// Iterates the first elements only
        /// </summary>
        public static IEnumerable<T> First<T>(this IEnumerable<T> list, int max)
        {
            if (list == null)
                throw new ArgumentNullException(nameof(list));

            int count = 0;

            foreach (T item in list)
            {
                yield return item;
                count++;

                if (count >= max)
                    break;
            }
        }

        public static IEnumerable<IList<T>> EnumerateBatches<T>(this IEnumerable<T> list, int batchSize)
        {
            if (list == null)
                throw new ArgumentNullException(nameof(list));

            List<T> batch = new List<T>();

            foreach (T item in list)
            {
                batch.Add(item);

                if (batch.Count >= batchSize)
                {
                    IList<T> b = batch;

                    batch = new List<T>();

                    yield return b;
                }
            }

            if (batch.Any())
                yield return batch;
        }

        /// <summary>
        /// Get all the pairs of items in a list
        /// </summary>
        public static IEnumerable<Tuple<T, T>> GetPairs<T>(this IList<T> list)
        {
            if (list == null)
                throw new ArgumentNullException(nameof(list));

            for (int x = 0; x < list.Count - 1; x++)
            {
                for (int y = x + 1; y < list.Count; y++)
                {
                    T e1 = list[x];
                    T e2 = list[y];

                    if (Equals(e1, e2))
                        continue;

                    Tuple<T, T> t = new Tuple<T, T>(e1, e2);

                    yield return t;
                }
            }
        }

        public static IList<T> ToIList<T>(this IEnumerable<T> source)
        {
            return source.ToList();
        }

        /// <summary>
        /// Adds items if not already in collection
        /// </summary>
        public static void TryAddRange<T>(this ISet<T> list, IEnumerable<T> items)
        {
            if (list == null)
                throw new ArgumentNullException(nameof(list));
            if (items == null)
                return;

            foreach (T item in items)
            {
                list.Add(item);
            }
        }

        /// <summary>
        /// Returns all distinct elements of the given source, where "distinctness"
        /// is determined via a projection and the default eqaulity comparer for the projected type.
        /// </summary>
        /// <remarks>
        /// This operator uses deferred execution and streams the results, although
        /// a set of already-seen keys is retained. If a key is seen multiple times,
        /// only the first element with that key is returned.
        /// </remarks>
        /// <typeparam name="TSource">Type of the source sequence</typeparam>
        /// <typeparam name="TKey">Type of the projected element</typeparam>
        /// <param name="source">Source sequence</param>
        /// <param name="keySelector">Projection for determining "distinctness"</param>
        /// <returns>A sequence consisting of distinct elements from the source sequence, comparing them by the specified key projection.</returns>
        public static IEnumerable<TSource> DistinctBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            return source.DistinctBy(keySelector, null);
        }

        /// <summary>
        /// Returns all distinct elements of the given source, where "distinctness"
        /// is determined via a projection and the specified comparer for the projected type.
        /// </summary>
        /// <remarks>
        /// This operator uses deferred execution and streams the results, although
        /// a set of already-seen keys is retained. If a key is seen multiple times,
        /// only the first element with that key is returned.
        /// </remarks>
        /// <typeparam name="TSource">Type of the source sequence</typeparam>
        /// <typeparam name="TKey">Type of the projected element</typeparam>
        /// <param name="source">Source sequence</param>
        /// <param name="keySelector">Projection for determining "distinctness"</param>
        /// <param name="comparer">The equality comparer to use to determine whether or not keys are equal.
        /// If null, the default equality comparer for <c>TSource</c> is used.</param>
        /// <returns>A sequence consisting of distinct elements from the source sequence, comparing them by the specified key projection.</returns>
        public static IEnumerable<TSource> DistinctBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, IEqualityComparer<TKey>? comparer)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));

            return DistinctByImpl(source, keySelector, comparer);
        }

        static IEnumerable<TSource> DistinctByImpl<TSource, TKey>(IEnumerable<TSource> source, Func<TSource, TKey> keySelector, IEqualityComparer<TKey>? comparer)
        {
            HashSet<TKey> knownKeys = new HashSet<TKey>(comparer);
            foreach (TSource element in source)
            {
                if (knownKeys.Add(keySelector(element)))
                {
                    yield return element;
                }
            }
        }

        #region IProducerConsumerCollection

        /// <summary>Clears the collection by repeatedly taking elements until it's empty.</summary>
        /// <typeparam name="T">Specifies the type of the elements in the collection.</typeparam>
        /// <param name="collection">The collection to be cleared.</param>
        public static void Clear<T>(this IProducerConsumerCollection<T> collection)
        {
            if (collection == null)
                throw new ArgumentNullException(nameof(collection));

            while (collection.TryTake(out _))
            {
            }
        }

        #endregion
    }
}