using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using SystemPlus;
using SystemPlus.IO;
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
                HttpWebRequest request = request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "GET";

                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    Stream receiveStream = response.GetFullResponseStream();
                    StreamReader sr = new StreamReader(receiveStream);

                    string responseMessage = sr.ReadToEnd();

                    OauthReponse oauthResponse = Serialization.JsonDeserialize<OauthReponse>(responseMessage);

                    if (oauthResponse == null || !oauthResponse.ok)
                        throw new Exception("Slack error: " + responseMessage);

                    return oauthResponse;
                }
            }
            catch (Exception ex)
            {
                ex.AddData("code", code);
                throw;
            }
        }

        public Task SendAsync(Payload payload, string urlWithAccessToken)
        {
            return Task.Run(() => Send(payload, urlWithAccessToken));
        }

        public void Send(Payload payload, string urlWithAccessToken)
        {
            string payloadJson = Serialization.JsonSerialize(payload);

            try
            {
                HttpWebRequest request = request = (HttpWebRequest)WebRequest.Create(urlWithAccessToken);
                request.Method = "POST";
                request.KeepAlive = true;
                request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
                request.ContentType = "application/json; charset=utf-8";

                request.WriteRequestStream(payloadJson, Encoding.UTF8);

                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    Stream receiveStream = response.GetFullResponseStream();
                    StreamReader sr = new StreamReader(receiveStream);

                    string responseMessage = sr.ReadToEnd();

                    if (responseMessage != "ok")
                        throw new Exception("Slack error: " + responseMessage);
                }
            }
            catch (WebException ex)
            {                
                if (ex.Response != null)
                {
                    Stream responseStream = ex.Response.GetResponseStream();
                    StreamReader sr = new StreamReader(responseStream);
                    string s = sr.ReadToEnd();
                    
                    if(string.Equals(s, "No service", StringComparison.InvariantCultureIgnoreCase))
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
        public static string EscapeMessage(string message)
        {
            message = message.Replace("&", "&amp;");
            message = message.Replace("<", "&lt;");
            message = message.Replace(">", "&gt;");

            return message;
        }
    }

}
