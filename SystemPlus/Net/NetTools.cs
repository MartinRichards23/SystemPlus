using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text.RegularExpressions;
using SystemPlus.Text;

namespace SystemPlus.Net
{
    public static class NetTools
    {
        /// <summary>
        /// Gets the part of email address after @
        /// </summary>
        public static string GetEmailDomain(string email)
        {
            int index = email.IndexOf("@", StringComparison.Ordinal);

            if (index < 0)
                throw new ArgumentException("Email does not contain '@'");
            
            return email.Substring(index + 1);
        }

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
        /// Is uri a mailto: uri
        /// </summary>
        public static bool IsEmailUri(string uri)
        {
            return uri.StartsWith("mailto:", StringComparison.InvariantCultureIgnoreCase);
        }

        /// <summary>
        /// Checks is a string is a valid email address
        /// </summary>
        public static bool VerifyEmail(string email)
        {
            Regex emailRegex = new Regex(@"^\S+@\S+\.\S+$", RegexOptions.IgnoreCase);
            return emailRegex.IsMatch(email);
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
            string domain = NetTools.GetDomain(url);

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
            return Uri.TryCreate(url, UriKind.Absolute, out Uri result);
        }

        /// <summary>
        /// Gets an absolute Url from a relative one and its base url
        /// </summary>
        public static string MakeAbsoluteUrl(string baseUrl, string relativeUrl)
        {
            if (IsAbsoluteUrl(relativeUrl))
                return relativeUrl;

            UriBuilder builder = new UriBuilder(baseUrl);

            Uri frameUri = new Uri(builder.Uri, relativeUrl);
            return frameUri.ToString();
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
                if (!Uri.TryCreate(url, UriKind.RelativeOrAbsolute, out Uri uri))
                    continue;

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

        public static List<Uri> FilterHttpUrls(this IEnumerable<Uri> uris)
        {
            List<Uri> filteredUris = new List<Uri>();

            foreach (Uri uri in uris)
            {
                if (uri.Scheme == "http" || uri.Scheme == "https")
                    filteredUris.Add(uri);
            }

            return filteredUris;
        }

        /// <summary>
        /// Gets an relative Url from an absolute one
        /// </summary>
        public static string MakeRelativeUrl(string url)
        {
            UriBuilder builder = new UriBuilder(url);
            return builder.Uri.PathAndQuery;
        }

        public static bool IsInternetAvailable()
        {
            try
            {
                Dns.GetHostEntry("www.google.com");
                return true;
            }
            catch
            {
                return false; // host not reachable.
            }
        }

        // this is one of the key problems with the Amazon code and C#.. C# by default returns excaped values in lower case
        // for example %3a but Amazon expects them in upper case i.e. %3A, this function changes them to upper case..
        //
        public static string UpperCaseUrlEncode(string s)
        {
            char[] temp = Uri.EscapeDataString(s).ToCharArray();
            for (int i = 0; i < temp.Length - 2; i++)
            {
                if (temp[i] == '%')
                {
                    temp[i + 1] = char.ToUpper(temp[i + 1]);
                    temp[i + 2] = char.ToUpper(temp[i + 2]);
                }
            }
            return new string(temp);
        }

        public static DateTime GetNetworkTime(string ntpServer = "time.windows.com")
        {
            // NTP message size - 16 bytes of the digest (RFC 2030)
            var ntpData = new byte[48];

            //Setting the Leap Indicator, Version Number and Mode values
            ntpData[0] = 0x1B; //LI = 0 (no warning), VN = 3 (IPv4 only), Mode = 3 (Client Mode)

            IPAddress[] addresses = Dns.GetHostEntry(ntpServer).AddressList;

            //The UDP port number assigned to NTP is 123
            IPEndPoint ipEndPoint = new IPEndPoint(addresses[0], 123);

            using (Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp))
            {
                socket.SendTimeout = 15000;
                socket.ReceiveTimeout = 15000;

                socket.Connect(ipEndPoint);
                socket.Send(ntpData);
                socket.Receive(ntpData);
            }

            //Offset to get to the "Transmit Timestamp" field (time at which the reply 
            //departed the server for the client, in 64-bit timestamp format."
            const byte serverReplyTime = 40;

            //Get the seconds part
            ulong intPart = BitConverter.ToUInt32(ntpData, serverReplyTime);

            //Get the seconds fraction
            ulong fractPart = BitConverter.ToUInt32(ntpData, serverReplyTime + 4);

            //Convert From big-endian to little-endian
            intPart = ByteTools.SwapEndianness(intPart);
            fractPart = ByteTools.SwapEndianness(fractPart);

            ulong milliseconds = (intPart * 1000) + ((fractPart * 1000) / 0x100000000L);

            //**UTC** time
            DateTime networkDateTime = (new DateTime(1900, 1, 1)).AddMilliseconds((long)milliseconds);

            return networkDateTime;
        }

        /// <summary>
        /// Returns value indicating if port is a valid number
        /// </summary>
        public static bool IsValidPortNumber(int value)
        {
            if (value < 1)
                return false;
            if (value > 65535)
                return false;

            return true;
        }
    }
}