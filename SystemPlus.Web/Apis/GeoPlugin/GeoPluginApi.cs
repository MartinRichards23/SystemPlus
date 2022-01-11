using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace SystemPlus.Web.GeoPlugin
{
    /// <summary>
    /// http://www.geoplugin.com/quickstart
    /// </summary>
    public class GeoPluginApi
    {
        readonly HttpClient client = new HttpClient();
        readonly string baseUrl = "http://www.geoplugin.net/json.gp?ip=";

        public async Task<GeoPluginResult?> GetIpData(string ipAddress)
        {
            Uri uri = new Uri(baseUrl + ipAddress);
            string data = await client.GetStringAsync(uri);

            GeoPluginResult? result = JsonSerializer.Deserialize<GeoPluginResult>(data);

            return result;
        }
    }
}
