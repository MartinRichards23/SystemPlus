using System;
using System.Collections.Generic;
using System.Net;
using System.Runtime.Serialization;
using System.Text;
using SystemPlus.IO;

namespace SystemPlus.Web.Google
{
    /// <summary>
    /// https://www.google.com/recaptcha/admin#site/343369990?setup
    /// </summary>
    public class ReCatchpa
    {
        readonly static string publicKey = "6LcGaXcUAAAAAIu-q5r7cGVV8FabLW6eccGaYuzy";
        readonly static string privateKey = "6LcGaXcUAAAAAN-WuAz3KLhPQz1cnjMFGwHB53d7";

        public bool Validate(string response)
        {
            if (string.IsNullOrWhiteSpace(response))
                return false;

            WebClient client = new WebClient();

            string googleReply = client.DownloadString(string.Format("https://www.google.com/recaptcha/api/siteverify?secret={0}&response={1}", privateKey, response));

            ReCaptchaClass captchaResponse = Serialization.JsonDeserialize<ReCaptchaClass>(googleReply);

            return captchaResponse.Success;
        }

    }

    [DataContract]
    class ReCaptchaClass
    {
        [DataMember(Name = "success")]
        public bool Success { get; set; }

        [DataMember(Name = "error-codes")]
        public string ErrorCodes { get; set; }
    }
}

