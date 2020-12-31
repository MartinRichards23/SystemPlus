using System;
using System.IO;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using SystemPlus.Net;

namespace SystemPlus.Web.GeoPlugin
{
    /// <summary>
    /// http://www.geoplugin.com/quickstart
    /// </summary>
    public class GeoPluginApi
    {
        readonly string baseUrl = "http://www.geoplugin.net/json.gp?ip=";

        public async Task<GeoPluginResult?> GetIpData(string ipAddress)
        {
            Uri uri = new Uri(baseUrl + ipAddress);

            HttpWebRequest request = WebRequest.CreateHttp(uri);
            request.Method = "GET";
            request.Timeout = 5000;

            using HttpWebResponse response = await request.GetHttpResponseAsync();
            using Stream receiveStream = response.GetResponseStream();
            using StreamReader sr = new StreamReader(receiveStream);

            string data = await sr.ReadToEndAsync();

            GeoPluginResult? result = JsonSerializer.Deserialize<GeoPluginResult>(data);

            return result;
        }
    }
}
