﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using SystemPlus.IO;

namespace SystemPlus.Net
{
    public static class Extensions
    {
        public static HttpWebResponse GetHttpResponse(this HttpWebRequest request)
        {
            return (HttpWebResponse)request.GetResponse();
        }

        public static HttpWebResponse GetHttpResponse(this HttpWebRequest request, CancellationToken token)
        {
            return (HttpWebResponse)request.GetResponse(token);
        }

        /// <summary>
        /// A WebRequest extension method that gets the WebRequest response or the WebException response.
        /// </summary>
        /// <returns>The WebRequest response or WebException response.</returns>
        public static WebResponse GetResponseSafe(this WebRequest request)
        {
            try
            {
                return request.GetResponse();
            }
            catch (WebException e)
            {
                return e.Response;
            }
        }

        public static MemoryStream GetFullResponseStream(this HttpWebResponse response, int maxLength = int.MaxValue)
        {
            using (Stream s = response.GetResponseStream())
            {
                MemoryStream ms = new MemoryStream();
                s.CopyTo(ms, 1024, maxLength);

                ms.Position = 0;
                return ms;
            }
        }

        /// <summary>
        /// Returns a WebResponse from a request, aborts if the token is cancelled
        /// </summary>
        public static WebResponse GetResponse(this WebRequest request, CancellationToken token)
        {
            IAsyncResult asyncResult = request.BeginGetResponse(null, null);

            WaitHandle.WaitAny(new[] { asyncResult.AsyncWaitHandle, token.WaitHandle });

            if (token.IsCancellationRequested)
            {
                // abort request if it was cancelled
                request.Abort();
                token.ThrowIfCancellationRequested();
            }

            WebResponse response = request.EndGetResponse(asyncResult);
            return response;
        }

        public static string DownloadString(this WebRequest request, CancellationToken token, int maxLength = int.MaxValue)
        {
            using (WebResponse response = request.GetResponse(token))
            {
                return response.ReadWebResponse(maxLength);
            }
        }

        public static IList<KeyValuePair<string, string>> GetHeaders(this WebHeaderCollection headers)
        {
            IList<KeyValuePair<string, string>> headerValues = new List<KeyValuePair<string, string>>();

            foreach (string key in headers.AllKeys)
            {
                string value = headers[key];
                headerValues.Add(new KeyValuePair<string, string>(key.ToLowerInvariant(), value));
            }

            return headerValues;
        }

        public static void WriteRequestStream(this WebRequest request, string content, Encoding encoding)
        {
            byte[] bytes = encoding.GetBytes(content);

            request.ContentLength = bytes.Length;

            using (Stream ps = request.GetRequestStream())
            {
                ps.Write(bytes);
            }
        }

        /// <summary>
        /// Gets the content of the webexception response message
        /// </summary>
        /// <param name="exception"></param>
        /// <returns></returns>
        public static string GetResponseContent(this WebException exception)
        {
            using (HttpWebResponse response = (HttpWebResponse)exception.Response)
            using (Stream responseStream = response.GetResponseStream())
            using (StreamReader sr = new StreamReader(responseStream, Encoding.ASCII))
            {
                return sr.ReadToEnd();
            }
        }

        /// <summary>
        /// Gets the text content of webresponse, using the correct encoding
        /// </summary>
        public static string ReadWebResponse(this WebResponse response, int maxLength = int.MaxValue)
        {
            using (MemoryStream rawdata = new MemoryStream())
            {
                // save data to a memorystream
                using (Stream rs = response.GetResponseStream())
                {
                    rs.CopyTo(rawdata, 1024, maxLength);
                }

                // first see if content length header has charset = calue
                string charset = null;
                string ctype = response.Headers["content-type"];
                if (ctype != null)
                {
                    int ind = ctype.IndexOf("charset=", StringComparison.InvariantCultureIgnoreCase);
                    if (ind > -1)
                        charset = ctype.Substring(ind + 8);
                }

                // if ContentType is null, or did not contain charset, we search in body
                if (string.IsNullOrEmpty(charset))
                {
                    rawdata.Seek(0, SeekOrigin.Begin);

                    StreamReader srr = new StreamReader(rawdata, Encoding.ASCII);
                    string meta = srr.ReadToEnd();

                    int startInd = meta.IndexOf("charset=", StringComparison.InvariantCultureIgnoreCase);
                    if (startInd != -1)
                    {
                        int endInd = meta.IndexOf("\"", startInd, StringComparison.InvariantCultureIgnoreCase);
                        if (endInd != -1)
                        {
                            int start = startInd + 8;
                            charset = meta.Substring(start, endInd - start + 1);
                            charset = charset.TrimEnd('>', '"');
                        }
                    }
                }

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
                    catch
                    {
                        encoding = Encoding.UTF8;
                    }
                }

                rawdata.Seek(0, SeekOrigin.Begin);

                StreamReader sr = new StreamReader(rawdata, encoding);

                return sr.ReadToEnd();
            }
        }

    }
}