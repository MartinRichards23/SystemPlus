using System.Linq;
using System.Text.RegularExpressions;

namespace SystemPlus.Net
{
    public class UriParts
    {
        #region Fields

        readonly string suffix;
        readonly string tld;
        readonly string domain;
        readonly string domainName;
        readonly string fullDomain;

        #endregion

        public UriParts(string fullDomain, string domain, string suffix)
        {
            this.fullDomain = fullDomain;
            this.domain = domain;
            this.suffix = suffix;
            domainName = domain.Substring(0, domain.Length - suffix.Length - 1);
            tld = suffix.Split('.').Last();
        }

        #region Properties

        /// <summary>
        /// e.g. "www.abc.blogs.co.uk" returns "co.uk"
        /// </summary>
        public string Suffix
        {
            get { return suffix; }
        }

        /// <summary>
        /// e.g. "www.abc.blogs.co.uk" returns "uk"
        /// </summary>
        public string Tld
        {
            get { return tld; }
        }

        /// <summary>
        /// e.g. "www.abc.blogs.co.uk" returns "blogs.co.uk"
        /// </summary>
        public string Domain
        {
            get { return domain; }
        }

        /// <summary>
        /// e.g. "www.abc.blogs.co.uk" returns "blogs"
        /// </summary>
        public string DomainName
        {
            get { return domainName; }
        }

        /// <summary>
        /// e.g. "www.abc.blogs.co.uk" returns "www.abc.blogs.co.uk"
        /// </summary>
        public string FullDomain
        {
            get { return fullDomain; }
        }

        /// <summary>
        /// e.g. "www.abc.blogs.co.uk" returns "abc.blogs.com"
        /// </summary>
        public string FullDomainClean
        {
            get
            {
                string d = fullDomain;
                return Regex.Replace(d, @"^www?[0-9]?\.", "", RegexOptions.IgnoreCase);
            }
        }

        #endregion
    }
}