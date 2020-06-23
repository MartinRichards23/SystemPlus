using System.IO;
using System.Net;
using System.Text;
using SystemPlus.IO;
using SystemPlus.Net;

namespace SystemPlus.Web.W3C
{
    /// <summary>
    /// W3C HTML validator
    /// https://github.com/validator/validator/wiki/Service:-Input:-POST-body
    /// </summary>
    public class W3CApi
    {
        public W3CResult ValidateHtml(string html)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://validator.w3.org/nu/?out=json");
            request.Method = "POST";
            request.UserAgent = UserAgents.MozillaUserAgent.AgentString;
            request.KeepAlive = true;
            request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
            request.ContentType = "text/html; charset=utf-8";

            request.WriteRequestStream(html, Encoding.UTF8);

            using HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            using Stream receiveStream = response.GetFullResponseStream();
            using StreamReader sr = new StreamReader(receiveStream);

            string json = sr.ReadToEnd();

            W3CResult result = Serialization.JsonDeserialize<W3CResult>(json);
            return result;
        }
    }
}
