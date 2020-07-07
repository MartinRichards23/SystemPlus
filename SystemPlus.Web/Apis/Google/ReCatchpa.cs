using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace SystemPlus.Web.Google
{
    /// <summary>
    /// https://www.google.com/recaptcha/admin#site/343369990?setup
    /// </summary>
    public class ReCatchpa
    {
        readonly string privateKey;

        public ReCatchpa(string privateKey)
        {
            this.privateKey = privateKey;
        }

        public bool Validate(string response)
        {
            if (string.IsNullOrWhiteSpace(response))
                return false;

            using WebClient client = new WebClient();
            string googleReply = client.DownloadString($"https://www.google.com/recaptcha/api/siteverify?secret={privateKey}&response={response}");

            ReCaptchaClass captchaResponse = JsonSerializer.Deserialize<ReCaptchaClass>(googleReply);

            return captchaResponse.Success;
        }
    }

    class ReCaptchaClass
    {
        [JsonPropertyName("success")]
        public bool Success { get; set; }

        [JsonPropertyName("error-codes")]
        public string? ErrorCodes { get; set; }
    }
}

