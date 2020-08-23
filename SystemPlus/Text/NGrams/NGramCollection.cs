using System;
using System.Collections.Generic;

namespace SystemPlus.Text.NGrams
{
    public class NGramCollection
    {
        public int GramLength { get; }
        readonly IList<NGram> grams = new List<NGram>();

        protected NGramCollection()
        { }

        public NGramCollection(int gramLength)
        {
            GramLength = gramLength;
        }

        public void Add(NGram gram)
        {
            grams.Add(gram);
        }

        public IEnumerable<NGram> Grams
        {
            get { return grams; }
        }

        public override string ToString()
        {
            return $"Gram length: {GramLength}, Count: {grams.Count}";
        }
    }

    public class UniqueNGramCollection
    {
        readonly int gramLength;
        readonly IDictionary<NGram, int> grams = new Dictionary<NGram, int>();

        protected UniqueNGramCollection()
        { }

        public UniqueNGramCollection(int gramLength)
        {
            this.gramLength = gramLength;
        }

        public void Add(NGram gram)
        {
            if (grams.ContainsKey(gram))
                grams[gram]++;
            else
                grams.Add(gram, 1);
        }

        public void Add(NGram gram, int count)
        {
            grams.Add(gram, count);
        }

        public int GramLength
        {
            get { return gramLength; }
        }

        public IDictionary<NGram, int> Grams
        {
            get { return grams; }
        }

        public override string ToString()
        {
            return $"Gram length: {GramLength}, Count: {grams.Count}";
        }

        public static UniqueNGramCollection Create(NGramCollection ngrams)
        {
            if (ngrams == null)
                throw new ArgumentNullException(nameof(ngrams));

            UniqueNGramCollection uniqueNgrams = new UniqueNGramCollection(ngrams.GramLength);

            foreach (NGram gram in ngrams.Grams)
            {
                uniqueNgrams.Add(gram);
            }

            return uniqueNgrams;
        }

        public static IEnumerable<UniqueNGramCollection> Create(IEnumerable<NGramCollection> ngrams)
        {
            List<UniqueNGramCollection> uniqueNgramCollections = new List<UniqueNGramCollection>();

            if (ngrams != null)
            {
                foreach (NGramCollection gram in ngrams)
                {
                    UniqueNGramCollection uniqueCollection = UniqueNGramCollection.Create(gram);
                    uniqueNgramCollections.Add(uniqueCollection);
                }
            }

            return uniqueNgramCollections;
        }
    }
}