﻿namespace SystemPlus.Collections.Generic
{
    /// <summary>
    /// Iterates through a list, going back to the start when at the end
    /// </summary>
    public class CircularEnumerator<T>
    {
        int currentPosition;
        readonly List<T> items;

        public CircularEnumerator(IEnumerable<T> items)
        {
            this.items = new List<T>(items);
        }

        public void GoToRandomPosition(Random random)
        {
            if (random == null)
                throw new ArgumentNullException(nameof(random));

            currentPosition = random.Next(0, items.Count - 1);
        }

        public T Next()
        {
            T item = items[currentPosition];

            currentPosition++;
            CheckPos();

            return item;
        }

        public T Previous()
        {
            T item = items[currentPosition];

            currentPosition--;
            CheckPos();

            return item;
        }

        public int Count
        {
            get { return items.Count; }
        }

        void CheckPos()
        {
            if (currentPosition >= items.Count)
                currentPosition = 0;
            if (currentPosition < 0)
                currentPosition = items.Count - 1;
        }
    }
}