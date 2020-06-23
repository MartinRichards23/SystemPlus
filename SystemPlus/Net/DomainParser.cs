using System;
using System.IO;
using SystemPlus.Collections.ObjectModel;
using SystemPlus.IO;
using SystemPlus.Net.Mail;

namespace SystemPlus.Net
{
    /// <summary>
    /// Tools for extracting the tld and domain out of a url or email address
    /// </summary>
    public class DomainParser
    {
        readonly KeyedCollection<UrlSuffix> suffixes = new KeyedCollection<UrlSuffix>();
        bool initialised;

        /// <summary>
        /// Load all the suffixes
        /// http://mxr.mozilla.org/mozilla-central/source/netwerk/dns/effective_tld_names.dat?raw=1
        /// </summary>
        public void Initialise(Stream file)
        {
            lock (suffixes)
            {
                if (initialised)
                    return;

                using (StreamReader sr = new StreamReader(file))
                {
                    foreach (string l in sr.EnumerateLines())
                    {
                        string line = NormaliseSuffix(l);

                        if (string.IsNullOrWhiteSpace(line))
                            continue;
                        if (line.StartsWith("//", StringComparison.Ordinal))
                            continue;
                        if (line.StartsWith("!", StringComparison.Ordinal))
                            continue;

                        bool starred = false;

                        if (line.StartsWith("*", StringComparison.Ordinal))
                        {
                            starred = true;
                            line = line.TrimStart('*', '.');
                        }

                        UrlSuffix s = new UrlSuffix(line, starred);

                        suffixes.Add(s);
                    }
                }

                initialised = true;
            }
        }

        /// <summary>
        /// Get the parts of a url, e.g. "com" or "co.uk"
        /// </summary>
        public UriParts ParseUrl(Uri uri)
        {
            if (!initialised)
                throw new Exception("Domain parser not initialised");

            string host = uri.Host;
            host = NormaliseSuffix(host);

            string[] domainParts = host.Split('.');

            for (int pos = 0; pos < domainParts.Length; pos++)
            {
                string tld = string.Join(".", domainParts, pos, domainParts.Length - pos);

                if (suffixes.Contains(tld))
                {
                    UrlSuffix s = suffixes[tld];

                    if (s.Starred)
                    {
                        pos--;
                        tld = string.Join(".", domainParts, pos, domainParts.Length - pos);
                    }

                    pos--;
                    string domain = string.Join(".", domainParts, pos, domainParts.Length - pos);

                    UriParts result = new UriParts(uri.DnsSafeHost, domain, tld);
                    return result;
                }
            }

            throw new ArgumentException("Suffix not found for: " + uri);
        }

        /// <summary>
        /// Get the parts of a email, e.g. "com" or "co.uk"
        /// </summary>
        public EmailParts ParseEmail(string email)
        {
            if (!initialised)
                throw new Exception("Domain parser not initialised");

            int index = email.IndexOf("@", StringComparison.Ordinal);

            if (index < 0)
                throw new ArgumentException("Email does not contain '@'");

            string domainPart = email.Substring(index + 1);
            string localPart = email.Substring(0, index);

            string[] domainParts = domainPart.Split('.');

            for (int pos = 0; pos < domainParts.Length; pos++)
            {
                string tld = string.Join(".", domainParts, pos, domainParts.Length - pos);

                if (suffixes.Contains(tld))
                {
                    UrlSuffix s = suffixes[tld];

                    if (s.Starred)
                    {
                        pos--;
                        tld = string.Join(".", domainParts, pos, domainParts.Length - pos);
                    }

                    pos--;
                    string domain = string.Join(".", domainParts, pos, domainParts.Length - pos);

                    EmailParts result = new EmailParts(localPart, domainPart, domain, tld);
                    return result;
                }
            }

            throw new ArgumentException("Suffix not found for: " + email);
        }

        static string NormaliseSuffix(string suffix)
        {
            return suffix.Trim().ToLowerInvariant();
        }

        sealed class UrlSuffix : IKeyed
        {
            public UrlSuffix(string suffix, bool starred)
            {
                Key = suffix;
                Starred = starred;
            }

            public bool Starred { get; }

            public string Key { get; }

            public override bool Equals(object? obj)
            {
                // if parameter cannot be cast return false
                if (obj is UrlSuffix other)
                {
                    return Key == other.Key;
                }

                return false;
            }

            public override int GetHashCode()
            {
                return HashCode.Combine(Key);
            }

            public override string ToString()
            {
                return Key;
            }

        }
    }
}