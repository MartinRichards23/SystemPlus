using System;

namespace SystemPlus.Web.Slack
{
    [Serializable]
    public class IncomingWebhook
    {
        public string url { get; set; }
        public string channel { get; set; }
        public string configuration_url { get; set; }
    }

    [Serializable]
    public class Bot
    {
        public string bot_user_id { get; set; }
        public string bot_access_token { get; set; }
    }

    [Serializable]
    public class OauthReponse
    {
        public bool ok { get; set; }
        public string error { get; set; }

        public string access_token { get; set; }
        public string scope { get; set; }
        public string team_name { get; set; }
        public string team_id { get; set; }
        public IncomingWebhook incoming_webhook { get; set; }
        public Bot bot { get; set; }
    }
}
