using System.Text.RegularExpressions;

namespace SystemPlus.Net.Mail
{
    /// <summary>
    /// Represents email components
    /// </summary>
    public class EmailParts
    {
        public EmailParts(string localPart, string fullDomain, string domain, string suffix)
        {
            LocalPart = localPart;
            FullDomain = fullDomain;
            Domain = domain;
            Suffix = suffix;
            DomainName = domain.Substring(0, domain.Length - suffix.Length - 1);
            Tld = suffix.Split('.').Last();
        }

        #region Properties

        /// <summary>
        /// e.g. "abc.123@xyz.co.uk" returns "abc.123"
        /// </summary>
        public string LocalPart { get; }

        /// <summary>
        /// e.g. "abc.123@xyz.co.uk" returns "co.uk"
        /// </summary>
        public string Suffix { get; }

        /// <summary>
        /// e.g. "abc.123@xyz.co.uk" returns "uk"
        /// </summary>
        public string Tld { get; }

        /// <summary>
        /// e.g. "abc.123@xyz.co.uk" returns "xyz.co.uk"
        /// </summary>
        public string Domain { get; }

        /// <summary>
        /// e.g. "abc.123@xyz.co.uk" returns "xyz"
        /// </summary>
        public string DomainName { get; }

        /// <summary>
        /// e.g. "abc.123@xyz.co.uk" returns "www.abc.blogs.co.uk"
        /// </summary>
        public string FullDomain { get; }

        /// <summary>
        /// e.g. "abc.123@xyz.co.uk" returns "abc.blogs.co.uk"
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