using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using SystemPlus.Collections.Generic;

namespace SystemPlus.Net
{
    /// <summary>
    /// A url query string helper class
    /// </summary>
    public class UrlQueryString
    {
        #region Fields

        readonly IList<UrlParam> parameters = new List<UrlParam>();

        #endregion

        public UrlQueryString(bool ignoreEmpty)
        {
            IgnoreEmpty = ignoreEmpty;
        }

        public UrlQueryString(string baseUrl)
        {
            BaseUrl = baseUrl;
        }

        public UrlQueryString(string baseUrl, bool ignoreEmpty)
        {
            BaseUrl = baseUrl;
            IgnoreEmpty = ignoreEmpty;
        }

        #region Properties

        public string BaseUrl { get; }

        public bool IgnoreEmpty { get; }

        /// <summary>
        /// overrides the default indexer
        /// </summary>
        /// <param name="index"></param>
        /// <returns>the associated decoded value for the specified index</returns>
        public string this[int index]
        {
            get { return parameters[index].Value; }
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Parses url parameters out of a url
        /// </summary>
        /// <param name="s">the string to parse</param>
        public void FillFromString(string s)
        {
            parameters.Clear();

            if (string.IsNullOrEmpty(s))
                return;

            string[] vals = ExtractQuerystring(s).Split('&');

            foreach (string keyValuePair in vals)
            {
                if (string.IsNullOrEmpty(keyValuePair))
                    continue;

                string[] split = keyValuePair.Split('=');

                UrlParam param = new UrlParam(split[0], split.Length == 2 ? split[1] : string.Empty);
                parameters.Add(param);
            }
        }

        public UrlQueryString Add(string name, object value)
        {
            string valString = value?.ToString() ?? string.Empty;

            Add(name, valString);

            return this;
        }

        /// <summary>
        /// adds a name value pair to the collection
        /// </summary>
        /// <param name="name">the name</param>
        /// <param name="value">the value associated to the name</param>
        public UrlQueryString Add(string name, string value)
        {
            UrlParam param = new UrlParam(name, value);
            parameters.Add(param);

            return this;
        }

        /// <summary>
        /// removes a name value pair from the querystring collection
        /// </summary>
        /// <param name="name">name of the querystring value to remove</param>
        public void Remove(string name)
        {
            parameters.RemoveWhere(p => p.Name == name);
        }

        /// <summary>
        /// Makes the full url with query string
        /// </summary>
        public string MakeUrl()
        {
            return BaseUrl + MakeQuery();
        }

        /// <summary>
        /// Makes the full url with query string
        /// </summary>
        public string MakeQuery(bool includeStart = true)
        {
            StringBuilder sb = new StringBuilder();

            if (parameters.IsNullOrEmpty())
                return sb.ToString();

            string paramChar = includeStart ? "?" : string.Empty;

            // build params string
            for (int i = 0; i < parameters.Count; i++)
            {
                UrlParam param = parameters[i];

                if (IgnoreEmpty && string.IsNullOrEmpty(param.Value))
                    continue;

                sb.Append(paramChar).Append(param.Name).Append("=").Append(param.ValueEncoded);

                paramChar = "&";
            }

            return sb.ToString();
        }

        /// <summary>
        /// Convert to Uri 
        /// </summary>
        public Uri ToUri()
        {
            string url = MakeUrl();
            return new Uri(url);
        }


        public override string ToString()
        {
            return MakeUrl();
        }

        #endregion

        #region Statics

        /// <summary>
        /// Extracts a querystring from a full Url
        /// </summary>
        /// <param name="s">the string to extract the querystring from</param>
        /// <returns>a string representing only the querystring</returns>
        public static string ExtractQuerystring(string s)
        {
            if (!string.IsNullOrEmpty(s))
            {
                if (s.Contains("?", StringComparison.InvariantCulture))
                    return s.Substring(s.IndexOf("?", StringComparison.InvariantCultureIgnoreCase) + 1);
            }
            return s;
        }

        /// <summary>
        /// Extracts the key=value pairs from a Url query string
        /// </summary>
        public static IEnumerable<KeyValuePair<string, string>> ParseKeyValues(string querystring)
        {
            IList<KeyValuePair<string, string>> values = new List<KeyValuePair<string, string>>();

            if (string.IsNullOrEmpty(querystring))
                return values;

            string[] parts = querystring.Split('&', '?');

            foreach (string part in parts)
            {
                int index = part.IndexOf('=', StringComparison.InvariantCulture);

                if (index < 0)
                    continue;

                string key = part.Substring(0, index);
                string val = part.Substring(index + 1);

                values.Add(new KeyValuePair<string, string>(key, val));
            }

            return values;
        }

        /// <summary>
        /// Implicitly convert to string 
        /// </summary>
        public static implicit operator string(UrlQueryString input)
        {
            return input.MakeUrl();
        }

        #endregion

        sealed class UrlParam
        {
            public UrlParam(string name, string value)
            {
                Name = name;
                Value = value;
            }

            public string Name { get; }

            public string Value { get; }

            public string ValueEncoded
            {
                get
                {
                    if (Value == null)
                        return string.Empty;

                    return Uri.EscapeDataString(Value);
                }
            }

            public override string ToString()
            {
                return $"Name: {Name} Value: {Value}";
            }
        }
    }
}