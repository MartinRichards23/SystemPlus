using System.Linq;
using System.Text.RegularExpressions;

namespace SystemPlus.Net.Mail
{
    public class EmailParts
    {
        #region Fields

        readonly string localPart;

        readonly string suffix;
        readonly string tld;
        readonly string domain;
        readonly string domainName;
        readonly string fullDomain;

        #endregion

        public EmailParts(string localPart, string fullDomain, string domain, string suffix)
        {
            this.localPart = localPart;
            this.fullDomain = fullDomain;
            this.domain = domain;
            this.suffix = suffix;
            domainName = domain.Substring(0, domain.Length - suffix.Length - 1);
            tld = suffix.Split('.').Last();
        }

        #region Properties

        /// <summary>
        /// e.g. "abc.123@xyz.co.uk" returns "abc.123"
        /// </summary>
        public string LocalPart
        {
            get { return localPart; }
        }

        /// <summary>
        /// e.g. "abc.123@xyz.co.uk" returns "co.uk"
        /// </summary>
        public string Suffix
        {
            get { return suffix; }
        }

        /// <summary>
        /// e.g. "abc.123@xyz.co.uk" returns "uk"
        /// </summary>
        public string Tld
        {
            get { return tld; }
        }

        /// <summary>
        /// e.g. "abc.123@xyz.co.uk" returns "xyz.co.uk"
        /// </summary>
        public string Domain
        {
            get { return domain; }
        }

        /// <summary>
        /// e.g. "abc.123@xyz.co.uk" returns "xyz"
        /// </summary>
        public string DomainName
        {
            get { return domainName; }
        }

        /// <summary>
        /// e.g. "abc.123@xyz.co.uk" returns "www.abc.blogs.co.uk"
        /// </summary>
        public string FullDomain
        {
            get { return fullDomain; }
        }

        /// <summary>
        /// e.g. "abc.123@xyz.co.uk" returns "abc.blogs.co.uk"
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