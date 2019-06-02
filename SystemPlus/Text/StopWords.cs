using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SystemPlus.Text
{
    public class StopWords
    {
        readonly HashSet<string> words = new HashSet<string>();

        public void LoadStream(Stream stream)
        {
            StreamReader sr = new StreamReader(stream, Encoding.UTF8);

            string line;
            while ((line = sr.ReadLine()) != null)
            {
                string word = line.Trim();

                if (string.IsNullOrWhiteSpace(word))
                    continue;

                if (word.StartsWith("//"))
                    continue;

                word = word.ToUpperInvariant();

                words.Add(word);
            }
        }

        public bool Contains(string word)
        {
            if (word == null)
                return false;

            word = word.ToUpperInvariant();

            return words.Contains(word);
        }
    }
}
