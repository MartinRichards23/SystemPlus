using System;
using System.Collections.Generic;
using System.Globalization;

namespace SystemPlus.Net
{
    /// <summary>
    /// Tools for manipulating Uris
    /// </summary>
    public static class UrlTools
    {
        /// <summary>
        /// Gets domain from url, i.e. gets "http://www.abc.com" from "http://www.abc.com"
        /// </summary>
        public static string GetFullDomain(string url)
        {
            UriBuilder builder = new UriBuilder(url);
            Uri uri = builder.Uri;
            return uri.Scheme + "://" + uri.DnsSafeHost;
        }

        /// <summary>
        /// Gets domain from url, i.e. gets "www.abc.com" from "http://www.abc.com/123"
        /// </summary>
        public static string GetDomain(string url)
        {
            UriBuilder builder = new UriBuilder(url);
            Uri uri = builder.Uri;
            return uri.DnsSafeHost;
        }

        /// <summary>
        /// Removes the anchor part of the url
        /// </summary>
        public static string StripAnchor(string url)
        {
            int pos = url.LastIndexOf("#", StringComparison.Ordinal);

            if (pos > 0)
                return url.Substring(0, pos);

            return url;
        }

        /// <summary>
        /// Removes the parameters
        /// </summary>
        public static string StripParameters(string url)
        {
            int pos = url.LastIndexOf("?", StringComparison.Ordinal);

            if (pos > 0)
                return url.Substring(0, pos);

            return url;
        }

        /// <summary>
        /// Turns http://www.abc.com into www.abc.com
        /// </summary>
        public static string StripProtocol(string url)
        {
            Uri uri = new Uri(url);
            return uri.Host + uri.PathAndQuery;
        }

        /// <summary>
        /// Is uri a data: uri
        /// </summary>
        public static bool IsDataUri(string uri)
        {
            return uri.StartsWith("data:", StringComparison.InvariantCultureIgnoreCase);
        }

        /// <summary>
        /// Checks is a string is potentially a valid url
        /// </summary>
        public static bool VerifyUrl(string url)
        {
            if (string.IsNullOrWhiteSpace(url))
                return false;

            if (!url.Contains("."))
                return false;

            return true;
        }

        /// <summary>
        /// Is url a google ad url or similar
        /// </summary>
        public static bool IsAdvertUrl(string url)
        {
            string domain = GetDomain(url);

            if (domain.Contains("googleadservices.com", StringComparison.InvariantCultureIgnoreCase))
                return true;
            if (domain.Contains("googlesyndication.com", StringComparison.InvariantCultureIgnoreCase))
                return true;
            if (domain.Contains("doubleclick.net", StringComparison.InvariantCultureIgnoreCase))
                return true;

            return false;
        }

        /// <summary>
        /// Is uri a javascript: uri
        /// </summary>
        public static bool IsJsUri(string uri)
        {
            return uri.StartsWith("javascript:", StringComparison.InvariantCultureIgnoreCase);
        }

        /// <summary>
        /// Determines if Url is a relative one
        /// </summary>
        public static bool IsAbsoluteUrl(string url)
        {
            return Uri.TryCreate(url, UriKind.Absolute, out _);
        }

        /// <summary>
        /// Gets an absolute Url from a relative one and its base url
        /// </summary>
        public static string MakeAbsoluteUrl(string baseUrl, string relativeUrl)
        {
            UriBuilder builder = new UriBuilder(baseUrl);
            return MakeAbsoluteUrl(builder.Uri, relativeUrl);
        }

        /// <summary>
        /// Gets an absolute Url from a relative one and its base url
        /// </summary>
        public static string MakeAbsoluteUrl(Uri baseUri, string relativeUrl)
        {
            if (IsAbsoluteUrl(relativeUrl))
                return relativeUrl;

            Uri frameUri = new Uri(baseUri, relativeUrl);
            return frameUri.ToString();
        }

        public static List<Uri> ToUris(this IEnumerable<string> urls)
        {
            List<Uri> uris = new List<Uri>();

            foreach (string url in urls)
            {
                if (Uri.TryCreate(url, UriKind.RelativeOrAbsolute, out Uri? uri))
                    uris.Add(uri);
            }

            return uris;
        }

        public static List<Uri> MakeAbsoluteUris(this IEnumerable<Uri> uris, Uri baseUri)
        {
            List<Uri> absoluteUris = new List<Uri>();

            foreach (Uri uri in uris)
            {
                if (uri.IsAbsoluteUri)
                {
                    absoluteUris.Add(uri);
                }
                else if (baseUri != null)
                {
                    Uri fullUri = new Uri(baseUri, uri);
                    absoluteUris.Add(fullUri);
                }
            }

            return absoluteUris;
        }

        public static IList<string> MakeAbsoluteUrls(this IEnumerable<string> urls, string baseUrl)
        {
            List<string> absoluteUrls = new List<string>();

            UriBuilder builder = new UriBuilder(baseUrl);
            Uri baseUri = builder.Uri;

            foreach (string url in urls)
            {
                string absoluteUrl = MakeAbsoluteUrl(baseUri, url);
                absoluteUrls.Add(absoluteUrl);
            }

            return absoluteUrls;
        }

        /// <summary>
        /// Gets an relative Url from an absolute one
        /// </summary>
        public static string MakeRelativeUrl(string url)
        {
            UriBuilder builder = new UriBuilder(url);
            return builder.Uri.PathAndQuery;
        }

        /// <summary>
        /// Url encode a string using upper case text
        /// </summary>
        public static string UpperCaseUrlEncode(string data)
        {
            char[] temp = Uri.EscapeDataString(data).ToCharArray();
            for (int i = 0; i < temp.Length - 2; i++)
            {
                if (temp[i] == '%')
                {
                    temp[i + 1] = char.ToUpper(temp[i + 1], CultureInfo.InvariantCulture);
                    temp[i + 2] = char.ToUpper(temp[i + 2], CultureInfo.InvariantCulture);
                }
            }
            return new string(temp);
        }

    }
}
