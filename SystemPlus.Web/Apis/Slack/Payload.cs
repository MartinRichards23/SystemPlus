using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace SystemPlus.Web.Slack
{
    /// <summary>
    /// https://api.slack.com/docs/messages/builder
    /// </summary>
    public class Payload
    {
        [JsonPropertyName("channel")]
        public string? Channel { get; set; }

        [JsonPropertyName("username")]
        public string? Username { get; set; }

        [JsonPropertyName("text")]
        public string? Text { get; set; }

        [JsonPropertyName("attachments")]
        public List<Attachment>? Attachments { get; set; }

        public string? TextPlain
        {
            get { return Text; }
            set { Text = SlackClient.EscapeMessage(value); }
        }
    }

    public class Attachment
    {
        [JsonPropertyName("fallback")]
        public string? Fallback { get; set; }

        [JsonPropertyName("color")]
        public string? Color { get; set; }

        [JsonPropertyName("pretext")]
        public string? Pretext { get; set; }

        [JsonPropertyName("author_name")]
        public string? AuthorName { get; set; }

        [JsonPropertyName("author_link")]
        public string? AuthorLink { get; set; }

        [JsonPropertyName("author_icon")]
        public string? AuthorIcon { get; set; }

        [JsonPropertyName("title")]
        public string? Title { get; set; }

        [JsonPropertyName("title_link")]
        public string? TitleLink { get; set; }

        [JsonPropertyName("text")]
        public string? Text { get; set; }

        [JsonPropertyName("fields")]
        public List<Field>? Fields { get; set; }

        [JsonPropertyName("image_url")]
        public string? ImageUrl { get; set; }

        [JsonPropertyName("thumb_url")]
        public string? ThumbUrl { get; set; }

        [JsonPropertyName("footer")]
        public string? Footer { get; set; }

        [JsonPropertyName("footer_icon")]
        public string? FooterIcon { get; set; }

        [JsonPropertyName("ts")]
        public long Ts { get; set; }

        public string? TextPlain
        {
            get { return Text; }
            set { Text = SlackClient.EscapeMessage(value); }
        }

        public string? TitlePlain
        {
            get { return Title; }
            set { Title = SlackClient.EscapeMessage(value); }
        }

        public string? PreTextPlain
        {
            get { return Pretext; }
            set { Pretext = SlackClient.EscapeMessage(value); }
        }

        public string? FooterPlain
        {
            get { return Footer; }
            set { Footer = SlackClient.EscapeMessage(value); }
        }
    }

    public class Field
    {
        [JsonPropertyName("title")]
        public string? Title { get; set; }

        [JsonPropertyName("value")]
        public string? Value { get; set; }

        [JsonPropertyName("short")]
        public bool @Short { get; set; }
    }

    [Serializable]
    public class NoServiceException : Exception
    {
        public NoServiceException()
        {

        }

        public NoServiceException(string message) : base(message)
        {

        }

        public NoServiceException(string message, Exception innerException) : base(message, innerException)
        {

        }

        protected NoServiceException(SerializationInfo serializationInfo, StreamingContext streamingContext) : base(serializationInfo, streamingContext)
        {

        }
    }
}
