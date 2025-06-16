using System.Collections.Specialized;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Web;

namespace SystemPlus.Net
{
    /// <summary>
    /// Tools for manipulating Uris
    /// </summary>
    [SuppressMessage("Design", "CA1054:Uri parameters should not be strings")]
    [SuppressMessage("Design", "CA1055:Uri return values should not be strings")]
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
        /// Turns http://www.abc.com into www.abc.com
        /// </summary>
        public static string StripProtocol(string url)
        {
            Uri uri = new Uri(url);
            return uri.Host + uri.PathAndQuery;
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

        public static List<Uri> MakeAbsoluteUris(this IEnumerable<Uri> uris, Uri baseUri)
        {
            List<Uri> absoluteUris = new List<Uri>();

            if (uris != null)
            {
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
            }

            return absoluteUris;
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

        public static void AddParameter(this UriBuilder uriBuilder, string paramName, string paramValue)
        {
            var query = HttpUtility.ParseQueryString(uriBuilder.Query);
            query.Add(paramName, paramValue);
            uriBuilder.Query = query.ToString();
        }

        public static void AddParameter(this UriBuilder uriBuilder, NameValueCollection values)
        {
            var query = HttpUtility.ParseQueryString(uriBuilder.Query);
            query.Add(values);
            uriBuilder.Query = query.ToString();
        }
    }
}
