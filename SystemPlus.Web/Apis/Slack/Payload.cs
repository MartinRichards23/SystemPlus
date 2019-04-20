using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace SystemPlus.Web.Slack
{
    /// <summary>
    /// https://api.slack.com/docs/messages/builder
    /// </summary>
    [DataContract]
    public class Payload
    {
        [DataMember]
        public string channel { get; set; }

        [DataMember]
        public string username { get; set; }

        [DataMember]
        public string text { get; set; }

        [DataMember]
        public List<Attachment> attachments { get; set; }

        public string TextPlain
        {
            set { text = SlackClient.EscapeMessage(value); }
        }
    }

    [DataContract]
    public class Attachment
    {
        [DataMember]
        public string fallback { get; set; }

        [DataMember]
        public string color { get; set; }

        [DataMember]
        public string pretext { get; set; }

        [DataMember]
        public string author_name { get; set; }

        [DataMember]
        public string author_link { get; set; }

        [DataMember]
        public string author_icon { get; set; }

        [DataMember]
        public string title { get; set; }

        [DataMember]
        public string title_link { get; set; }

        [DataMember]
        public string text { get; set; }

        [DataMember]
        public List<Field> fields { get; set; }

        [DataMember]
        public string image_url { get; set; }

        [DataMember]
        public string thumb_url { get; set; }

        [DataMember]
        public string footer { get; set; }

        [DataMember]
        public string footer_icon { get; set; }

        [DataMember]
        public long ts { get; set; }

        public string TextPlain
        {
            set { text = SlackClient.EscapeMessage(value); }
        }

        public string TitlePlain
        {
            set { title = SlackClient.EscapeMessage(value); }
        }

        public string PreTextPlain
        {
            set { pretext = SlackClient.EscapeMessage(value); }
        }

        public string FooterPlain
        {
            set { footer = SlackClient.EscapeMessage(value); }
        }
    }

    [DataContract]
    public class Field
    {
        [DataMember]
        public string title { get; set; }

        [DataMember]
        public string value { get; set; }

        [DataMember]
        public bool @short { get; set; }
    }

    [Serializable]
    public class NoServiceException : Exception
    {

    }
}
