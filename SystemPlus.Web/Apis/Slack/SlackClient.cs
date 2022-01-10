using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using SystemPlus.Net;

namespace SystemPlus.Web.Slack
{
    public class SlackClient
    {
        readonly HttpClient client = new HttpClient();

        public SlackClient()
        {

        }

        public async Task<OauthReponse> OauthAccess(string clientId, string clientSecret, string code)
        {
            UrlQueryString url = new UrlQueryString("https://slack.com/api/oauth.access");
            url.Add("client_id", clientId);
            url.Add("client_secret", clientSecret);
            url.Add("code", code);

            try
            {
                string response = await client.GetStringAsync(url);

                OauthReponse? oauthResponse = JsonSerializer.Deserialize<OauthReponse>(response);

                if (oauthResponse == null || !oauthResponse.Ok)
                    throw new WebException("Slack error: " + response);

                return oauthResponse;
            }
            catch (Exception ex)
            {
                ex.AddData("code", code);
                throw;
            }
        }

        public async Task SendAsync(Payload payload, Uri urlWithAccessToken)
        {
            string payloadJson = JsonSerializer.Serialize(payload);

            try
            {
                StringContent content = new StringContent(payloadJson, Encoding.UTF8, "application/json");
                var responsed = await client.PostAsync(urlWithAccessToken, content);
                string responseMessage = await responsed.Content.ReadAsStringAsync();

                if (responseMessage != "ok")
                    throw new WebException("Slack error: " + responseMessage);
            }
            catch (WebException ex)
            {
                if (ex.Response != null)
                {
                    Stream responseStream = ex.Response.GetResponseStream();
                    StreamReader sr = new StreamReader(responseStream);
                    string s = await sr.ReadToEndAsync();

                    if (string.Equals(s, "No service", StringComparison.OrdinalIgnoreCase))
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
