using System;

namespace SystemPlus.Web.GeoPlugin
{
    [Serializable]
    public class GeoPluginResult
    {
        public string? geoplugin_request { get; set; }
        public int geoplugin_status { get; set; }
        public string? geoplugin_credit { get; set; }
        public string? geoplugin_city { get; set; }
        public string? geoplugin_region { get; set; }
        public string? geoplugin_areaCode { get; set; }
        public string? geoplugin_dmaCode { get; set; }
        public string? geoplugin_countryCode { get; set; }
        public string? geoplugin_countryName { get; set; }
        public string? geoplugin_continentCode { get; set; }
        public string? geoplugin_latitude { get; set; }
        public string? geoplugin_longitude { get; set; }
        public string? geoplugin_regionCode { get; set; }
        public string? geoplugin_regionName { get; set; }
        public string? geoplugin_currencyCode { get; set; }
        public string? geoplugin_currencySymbol { get; set; }
        public string? geoplugin_currencySymbol_UTF8 { get; set; }
        public double geoplugin_currencyConverter { get; set; }

        public string GetBestCurrency(string defaultValue)
        {
            if (geoplugin_currencyCode == "GBP" || geoplugin_currencyCode == "USD" || geoplugin_currencyCode == "EUR")
            {
                return geoplugin_currencyCode;
            }

            if (geoplugin_continentCode == "EU")
                return "EUR";

            return defaultValue;
        }
    }
}
