using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace SystemPlus.Text.NGrams
{
    /// <summary>
    /// Class that holds data for a single ngram
    /// </summary>
    [DataContract]
    public class NGram
    {
        #region Fields

        [DataMember]
        readonly string[] words;

        string? text;

        #endregion

        public NGram(string[] words)
        {
            this.words = words;
        }

        public string[] Words
        {
            get { return words; }
        }

        public string Text
        {
            get
            {
                if (text == null)
                    text = string.Join(" ", words);

                return text;
            }
        }

        public override string ToString()
        {
            return Text;
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as NGram);
        }

        public bool Equals(NGram? obj)
        {
            if (obj == null)
                return false;

            return Text.Equals(obj.Text, StringComparison.InvariantCulture);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Text);
        }

        #region Static functions

        /// <summary>
        /// Gets ngrams of varying lengths
        /// </summary>
        public static IEnumerable<NGramCollection> GetNGrams(IList<string> words, int minLength, int maxLength)
        {
            IList<NGramCollection> allNGrams = new List<NGramCollection>();

            for (int i = minLength; i <= maxLength; i++)
            {
                NGramCollection ngrams = GetNGrams(words, i);
                allNGrams.Add(ngrams);
            }

            return allNGrams;
        }

        /// <summary>
        /// Gets ngrams of given length
        /// </summary>
        public static NGramCollection GetNGrams(IList<string> words, int length)
        {
            NGramCollection ngrams = new NGramCollection(length);

            for (int i = 0; i < words.Count; i++)
            {
                if (i + length - 1 >= words.Count)
                    break;

                string[] gram = new string[length];

                for (int j = 0; j < length; j++)
                {
                    gram[j] = words[i + j];
                }

                ngrams.Add(new NGram(gram));
            }

            return ngrams;
        }

        #endregion
    }
}