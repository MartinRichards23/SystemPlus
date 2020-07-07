using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace SystemPlus.Web.Slack
{
    [SuppressMessage("Design", "CA1056:Uri properties should not be strings")]
    public class IncomingWebhook
    {
        [JsonPropertyName("url")]
        public string? Url { get; set; }

        [JsonPropertyName("channel")]
        public string? Channel { get; set; }

        [JsonPropertyName("configuration_url")]
        public string? ConfigurationUrl { get; set; }
    }

    public class Bot
    {
        [JsonPropertyName("bot_user_id")]
        public string? BotUserId { get; set; }

        [JsonPropertyName("bot_access_token")]
        public string? BotAccessToken { get; set; }
    }

    public class OauthReponse
    {
        [JsonPropertyName("ok")]
        public bool Ok { get; set; }

        [JsonPropertyName("error")]
        public string? Error { get; set; }

        [JsonPropertyName("access_token")]
        public string? AccessToken { get; set; }

        [JsonPropertyName("scope")]
        public string? Scope { get; set; }

        [JsonPropertyName("team_name")]
        public string? TeamName { get; set; }

        [JsonPropertyName("team_id")]
        public string? TeamId { get; set; }

        [JsonPropertyName("incoming_webhook")]
        public IncomingWebhook? IncomingWebhook { get; set; }

        [JsonPropertyName("bot")]
        public Bot? Bot { get; set; }
    }
}
