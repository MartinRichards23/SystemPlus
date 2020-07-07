using System.Text.Json.Serialization;

namespace SystemPlus.Web.GeoPlugin
{
    public class GeoPluginResult
    {
        [JsonPropertyName("geoplugin_request")]
        public string? Request { get; set; }

        [JsonPropertyName("geoplugin_status")]
        public int Status { get; set; }

        [JsonPropertyName("geoplugin_credit")]
        public string? Credit { get; set; }

        [JsonPropertyName("geoplugin_city")]
        public string? City { get; set; }

        [JsonPropertyName("geoplugin_region")]
        public string? Region { get; set; }

        [JsonPropertyName("geoplugin_areaCode")]
        public string? AreaCode { get; set; }

        [JsonPropertyName("geoplugin_dmaCode")]
        public string? DmaCode { get; set; }

        [JsonPropertyName("geoplugin_countryCode")]
        public string? CountryCode { get; set; }

        [JsonPropertyName("geoplugin_countryName")]
        public string? CountryName { get; set; }

        [JsonPropertyName("geoplugin_continentCode")]
        public string? ContinentCode { get; set; }

        [JsonPropertyName("geoplugin_latitude")]
        public string? Latitude { get; set; }

        [JsonPropertyName("geoplugin_longitude")]
        public string? Longitude { get; set; }

        [JsonPropertyName("geoplugin_regionCode")]
        public string? RegionCode { get; set; }

        [JsonPropertyName("geoplugin_regionName")]
        public string? RegionName { get; set; }

        [JsonPropertyName("geoplugin_currencyCode")]
        public string? CurrencyCode { get; set; }

        [JsonPropertyName("geoplugin_currencySymbol")]
        public string? CurrencySymbol { get; set; }

        [JsonPropertyName("geoplugin_currencySymbol_UTF8")]
        public string? CurrencySymbolUTF8 { get; set; }

        [JsonPropertyName("geoplugin_currencyConverter")]
        public double CurrencyConverter { get; set; }
    }
}
