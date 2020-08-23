using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace SystemPlus.Net
{
    /// <summary>
    /// Represents domain parts
    /// </summary>
    public class UriParts
    {
        public UriParts(string fullDomain, string domain, string suffix)
        {
            if (domain == null)
                throw new ArgumentNullException(nameof(domain));
            if (suffix == null)
                throw new ArgumentNullException(nameof(suffix));

            FullDomain = fullDomain;
            Domain = domain;
            Suffix = suffix;
            DomainName = domain.Substring(0, domain.Length - suffix.Length - 1);
            Tld = suffix.Split('.').Last();
        }

        #region Properties

        /// <summary>
        /// e.g. "www.abc.blogs.co.uk" returns "co.uk"
        /// </summary>
        public string Suffix { get; }

        /// <summary>
        /// e.g. "www.abc.blogs.co.uk" returns "uk"
        /// </summary>
        public string Tld { get; }

        /// <summary>
        /// e.g. "www.abc.blogs.co.uk" returns "blogs.co.uk"
        /// </summary>
        public string Domain { get; }

        /// <summary>
        /// e.g. "www.abc.blogs.co.uk" returns "blogs"
        /// </summary>
        public string DomainName { get; }

        /// <summary>
        /// e.g. "www.abc.blogs.co.uk" returns "www.abc.blogs.co.uk"
        /// </summary>
        public string FullDomain { get; }

        /// <summary>
        /// e.g. "www.abc.blogs.co.uk" returns "abc.blogs.com"
        /// </summary>
        public string FullDomainClean
        {
            get
            {
                string d = FullDomain;
                return Regex.Replace(d, @"^www?[0-9]?\.", "", RegexOptions.IgnoreCase);
            }
        }

        #endregion
    }
}