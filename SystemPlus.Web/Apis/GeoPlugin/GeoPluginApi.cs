using System.IO;
using System.Net;
using SystemPlus.IO;

namespace SystemPlus.Web.GeoPlugin
{
    /// <summary>
    /// http://www.geoplugin.com/quickstart
    /// </summary>
    public class GeoPluginApi
    {
        public GeoPluginResult GetIpData(string ipAddress)
        {
            string url = "http://www.geoplugin.net/json.gp?ip=" + ipAddress;

            HttpWebRequest request = WebRequest.CreateHttp(url);
            request.Method = "GET";
            request.Timeout = 5000;

            using HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            using Stream receiveStream = response.GetResponseStream();
            using StreamReader sr = new StreamReader(receiveStream);

            string data = sr.ReadToEnd();

            GeoPluginResult result = Serialization.JsonDeserialize<GeoPluginResult>(data);

            return result;
        }
    }
}
