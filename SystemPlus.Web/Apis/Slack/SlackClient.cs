using System;
using System.IO;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using SystemPlus.Net;

namespace SystemPlus.Web.Slack
{
    // https://api.slack.com/incoming-webhooks
    // https://api.slack.com/docs/oauth
    // https://api.slack.com/docs/slack-button

    public class SlackClient
    {
        public SlackClient()
        {

        }

        public OauthReponse OauthAccess(string clientId, string clientSecret, string code)
        {
            UrlQueryString url = new UrlQueryString("https://slack.com/api/oauth.access");
            url.Add("client_id", clientId);
            url.Add("client_secret", clientSecret);
            url.Add("code", code);

            try
            {
                HttpWebRequest request = request = (HttpWebRequest)WebRequest.Create(url.ToUri());
                request.Method = "GET";

                using HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                using Stream receiveStream = response.GetFullResponseStream();
                using StreamReader sr = new StreamReader(receiveStream);

                string responseMessage = sr.ReadToEnd();

                OauthReponse oauthResponse = JsonSerializer.Deserialize<OauthReponse>(responseMessage);

                if (oauthResponse == null || !oauthResponse.Ok)
                    throw new Exception("Slack error: " + responseMessage);

                return oauthResponse;
            }
            catch (Exception ex)
            {
                ex.AddData("code", code);
                throw;
            }
        }

        public async Task SendAsync(Payload payload, string urlWithAccessToken)
        {
            string payloadJson = JsonSerializer.Serialize(payload);

            try
            {
                HttpWebRequest request = request = (HttpWebRequest)WebRequest.Create(urlWithAccessToken);
                request.Method = "POST";
                request.KeepAlive = true;
                request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
                request.ContentType = "application/json; charset=utf-8";
                request.WriteRequestStream(payloadJson, Encoding.UTF8);

                using HttpWebResponse response = await request.GetHttpResponseAsync();
                using Stream receiveStream = response.GetFullResponseStream();
                using StreamReader sr = new StreamReader(receiveStream);

                string responseMessage = await sr.ReadToEndAsync();

                if (responseMessage != "ok")
                    throw new Exception("Slack error: " + responseMessage);
            }
            catch (WebException ex)
            {
                if (ex.Response != null)
                {
                    Stream responseStream = ex.Response.GetResponseStream();
                    StreamReader sr = new StreamReader(responseStream);
                    string s = await sr.ReadToEndAsync();

                    if (string.Equals(s, "No service", StringComparison.InvariantCultureIgnoreCase))
                        throw new NoServiceException();
                }

                throw;
            }
            catch (Exception ex)
            {
                ex.AddData("Uri", urlWithAccessToken);

                throw;
            }
        }

        /// <summary>
        /// Formats message
        /// https://api.slack.com/docs/message-formatting
        /// </summary>
        public static string? EscapeMessage(string? message)
        {
            if (message == null)
                return null;

            message = message.Replace("&", "&amp;", StringComparison.InvariantCulture);
            message = message.Replace("<", "&lt;", StringComparison.InvariantCulture);
            message = message.Replace(">", "&gt;", StringComparison.InvariantCulture);

            return message;
        }
    }

}
