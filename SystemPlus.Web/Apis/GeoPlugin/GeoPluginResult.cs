using System;
using System.Runtime.Serialization;

namespace SystemPlus.Web.GeoPlugin
{
    [DataContract]
    public class GeoPluginResult
    {
        [DataMember]
        public string? geoplugin_request { get; set; }

        [DataMember]
        public int geoplugin_status { get; set; }

        [DataMember]
        public string? geoplugin_credit { get; set; }

        [DataMember]
        public string? geoplugin_city { get; set; }

        [DataMember]
        public string? geoplugin_region { get; set; }

        [DataMember]
        public string? geoplugin_areaCode { get; set; }

        [DataMember]
        public string? geoplugin_dmaCode { get; set; }

        [DataMember]
        public string? geoplugin_countryCode { get; set; }

        [DataMember]
        public string? geoplugin_countryName { get; set; }

        [DataMember]
        public string? geoplugin_continentCode { get; set; }

        [DataMember]
        public string? geoplugin_latitude { get; set; }

        [DataMember]
        public string? geoplugin_longitude { get; set; }

        [DataMember]
        public string? geoplugin_regionCode { get; set; }

        [DataMember]
        public string? geoplugin_regionName { get; set; }

        [DataMember]
        public string? geoplugin_currencyCode { get; set; }

        [DataMember]
        public string? geoplugin_currencySymbol { get; set; }

        [DataMember]
        public string? geoplugin_currencySymbol_UTF8 { get; set; }

        [DataMember]
        public double geoplugin_currencyConverter { get; set; }
    }
}
