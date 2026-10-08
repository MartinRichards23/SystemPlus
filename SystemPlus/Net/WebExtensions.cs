using System.IO;
using System.Net;

namespace SystemPlus.Net
{
    /// <summary>
    /// Extension methods for web related classes
    /// </summary>
    public static class WebExtensions
    {
        public static IList<KeyValuePair<string, string>> GetHeaders(this WebHeaderCollection headers)
        {
            List<KeyValuePair<string, string>> headerValues = new List<KeyValuePair<string, string>>();

            if (headers != null)
            {
                foreach (string key in headers.AllKeys)
                {
                    string? value = headers[key];

                    if (value != null)
                        headerValues.Add(new KeyValuePair<string, string>(key, value));
                }
            }

            return headerValues;
        }

        /// <summary>
        /// Gets the content of the webexception response message
        /// </summary>
        /// <param name="exception"></param>
        /// <returns></returns>
        public static string GetResponseContent(this WebException exception)
        {
            ArgumentNullException.ThrowIfNull(exception);

            using WebResponse? response = exception.Response;

            if (response == null)
                throw new InvalidOperationException("The WebException does not contain a response.");

            using Stream responseStream = response.GetResponseStream();
            using StreamReader sr = new StreamReader(responseStream, Encoding.ASCII);

            return sr.ReadToEnd();
        }

        public static string? GetCharSet(WebHeaderCollection headers)
        {
            ArgumentNullException.ThrowIfNull(headers);

            string? charset = null;
            string? ctype = headers["content-type"];
            if (ctype != null)
            {
                int ind = ctype.IndexOf("charset=", StringComparison.InvariantCultureIgnoreCase);
                if (ind > -1)
                    charset = ctype[(ind + 8)..];
            }

            return charset;
        }

        public static string? GetCharSetFromBody(Stream rawdata)
        {
            if (rawdata == null)
                throw new ArgumentNullException(nameof(rawdata));

            rawdata.Seek(0, SeekOrigin.Begin);

            // the charset declaration must appear near the start of the document, so only scan the head
            const int maxScanLength = 4096;
            byte[] buffer = new byte[maxScanLength];
            int read = rawdata.ReadAtLeast(buffer, maxScanLength, throwOnEndOfStream: false);
            string meta = Encoding.ASCII.GetString(buffer, 0, read);

            int startInd = meta.IndexOf("charset=", StringComparison.OrdinalIgnoreCase);
            if (startInd == -1)
                return null;

            int start = startInd + "charset=".Length;

            while (start < meta.Length && (meta[start] == '"' || meta[start] == '\'' || char.IsWhiteSpace(meta[start])))
                start++;

            int end = start;
            while (end < meta.Length && (char.IsLetterOrDigit(meta[end]) || meta[end] == '-' || meta[end] == '_' || meta[end] == '.' || meta[end] == ':'))
                end++;

            if (end == start)
                return null;

            return meta[start..end];
        }

        public static Encoding GetEncoding(string? charset)
        {
            Encoding encoding;
            if (string.IsNullOrWhiteSpace(charset))
            {
                encoding = Encoding.UTF8; //default encoding
            }
            else
            {
                try
                {
                    encoding = Encoding.GetEncoding(charset);
                }
                catch (ArgumentException)
                {
                    encoding = Encoding.UTF8;
                }
            }

            return encoding;
        }
    }
}