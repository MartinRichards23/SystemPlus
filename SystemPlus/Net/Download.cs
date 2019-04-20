using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using SystemPlus.ComponentModel;
using SystemPlus.IO;
using SystemPlus.Text;

namespace SystemPlus.Net
{
    public static class Download
    {
        public static HttpWebRequest MakeHttpWebRequest(string url)
        {
            UriBuilder builder = new UriBuilder(url);
            return (HttpWebRequest)WebRequest.Create(builder.Uri);
        }

        public static HttpWebRequest MakeStandardGetRequest(string url)
        {
            HttpWebRequest request = MakeHttpWebRequest(url);

            request.Method = "GET";
            request.KeepAlive = true;
            request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;

            return request;
        }

        public static Stream Get(string url)
        {
            HttpWebRequest request = MakeStandardGetRequest(url);
            request.UserAgent = UserAgents.MozillaUserAgent.AgentString;

            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            {
                return response.GetFullResponseStream();
            }
        }

        public static byte[] GetBytes(string url)
        {
            HttpWebRequest request = MakeStandardGetRequest(url);
            request.UserAgent = UserAgents.MozillaUserAgent.AgentString;

            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            using (Stream stream = response.GetFullResponseStream())
            {
                return stream.ToBytes();
            }
        }

        /// <summary>
        /// Downloads the text from a url using a simple GET request
        /// </summary>
        public static string GetString(string url)
        {
            HttpWebRequest request = MakeStandardGetRequest(url);
            request.UserAgent = UserAgents.MozillaUserAgent.AgentString;

            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            {
                return response.ReadWebResponse();
            }
        }

        /// <summary>
        /// Follows a urls and returns the redirected url
        /// </summary>
        public static string GetUrlRedirect(string url, CancellationToken token)
        {
            HttpWebRequest request = MakeStandardGetRequest(url);
            request.AllowAutoRedirect = false;
            request.Timeout = 15 * 1000;

            using (HttpWebResponse response = request.GetHttpResponse(token))
            {
                string redirUrl = response.Headers["Location"];
                return redirUrl;
            }
        }

        const int bufferSize = 16 * 1024;

        public static void DownloadFile(Uri url, string outputFilePath, IProgressToken token)
        {
            try
            {
                using (FileStream outputFileStream = File.Create(outputFilePath, bufferSize))
                {
                    DownloadFile(url, outputFileStream, token);
                }
            }
            catch
            {
                File.Delete(outputFilePath);
                throw;
            }
        }

        public static void DownloadFile(Uri url, Stream outputStream, IProgressToken token)
        {
            HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(url);
            req.UserAgent = UserAgents.MozillaUserAgent.AgentString;

            using (WebResponse response = req.GetResponse())
            using (Stream responseStream = response.GetResponseStream())
            {
                int contentLength = response.Headers.Get("Content-Length").ToInt(-1);

                if (contentLength < 0)
                {
                    token.IsIndeterminate = true;
                }
                else
                {
                    token.IsIndeterminate = false;
                }

                byte[] buffer = new byte[bufferSize];

                int bytesRead;
                do
                {
                    token.ThrowIfCancellationRequested();
                    long length = response.ContentLength;
                    long position = outputStream.Length;

                    if (contentLength < 0)
                    {
                        token.UpdateStatus("{0}", StringTools.FormatBytes(position, "0.0"));
                    }
                    else
                    {
                        token.UpdateStatus("{0} of {1}", StringTools.FormatBytes(position, "0.0"), StringTools.FormatBytes(contentLength, "0.0"));
                        token.UpdateProgress(position, contentLength);
                    }

                    bytesRead = responseStream.Read(buffer, 0, bufferSize);
                    outputStream.Write(buffer, 0, bytesRead);
                } while (bytesRead > 0);
            }
        }
    }
}
