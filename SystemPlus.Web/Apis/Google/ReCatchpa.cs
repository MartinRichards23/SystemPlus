using System.Diagnostics.CodeAnalysis;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SystemPlus.Web.Google
{
    /// <summary>
    /// https://www.google.com/recaptcha/admin#site/343369990?setup
    /// </summary>
    public class ReCatchpa
    {
        readonly string privateKey;
        readonly HttpClient client = new HttpClient();

        public ReCatchpa(string privateKey)
        {
            this.privateKey = privateKey;
        }

        public async Task<bool> Validate(string response)
        {
            if (string.IsNullOrWhiteSpace(response))
                return false;

            string googleReply = await client.GetStringAsync($"https://www.google.com/recaptcha/api/siteverify?secret={privateKey}&response={response}");

            ReCaptchaClass? captchaResponse = JsonSerializer.Deserialize<ReCaptchaClass>(googleReply);

            if (captchaResponse == null)
                return false;

            return captchaResponse.Success;
        }
    }

    [SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses", Justification = "Serializable")]
    class ReCaptchaClass
    {
        [JsonPropertyName("success")]
        public bool Success { get; set; }

        [JsonPropertyName("error-codes")]
        public string? ErrorCodes { get; set; }
    }
}

