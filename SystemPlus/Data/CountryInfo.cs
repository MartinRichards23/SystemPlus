﻿namespace SystemPlus.Data
{
    /// <summary>
    /// Provides a list of all countries with the ISO 3166 code
    /// </summary>
    public class CountryInfo
    {
        public CountryInfo(int id, string iso2, string iso3, string name)
        {
            Id = id;
            Iso2 = iso2;
            Iso3 = iso3;
            Name = name;
        }

        /// <summary>
        /// ISO 3166 numeric code
        /// </summary>
        public int Id { get; }

        /// <summary>
        /// ISO 3166 2 letter code
        /// </summary>
        public string Iso2 { get; }

        /// <summary>
        /// ISO 3166 3 letter code
        /// </summary>
        public string Iso3 { get; }

        /// <summary>
        /// Country name
        /// </summary>
        public string Name { get; }

        public override string ToString()
        {
            return $"Id: {Iso3}, Name: {Name}";
        }

        public static IEnumerable<CountryInfo> GetCountries()
        {
            List<CountryInfo> countryModels = new List<CountryInfo>()
            {
                new CountryInfo(4, "AF", "AFG", "Afghanistan"),
new CountryInfo(8, "AL", "ALB", "Albania"),
new CountryInfo(12, "DZ", "DZA", "Algeria"),
new CountryInfo(16, "AS", "ASM", "American Samoa"),
new CountryInfo(20, "AD", "AND", "Andorra"),
new CountryInfo(24, "AO", "AGO", "Angola"),
new CountryInfo(660, "AI", "AIA", "Anguilla"),
new CountryInfo(10, "AQ", "ATA", "Antarctica"),
new CountryInfo(28, "AG", "ATG", "Antigua and Barbuda"),
new CountryInfo(32, "AR", "ARG", "Argentina"),
new CountryInfo(51, "AM", "ARM", "Armenia"),
new CountryInfo(533, "AW", "ABW", "Aruba"),
new CountryInfo(36, "AU", "AUS", "Australia"),
new CountryInfo(40, "AT", "AUT", "Austria"),
new CountryInfo(31, "AZ", "AZE", "Azerbaijan"),
new CountryInfo(44, "BS", "BHS", "Bahamas"),
new CountryInfo(48, "BH", "BHR", "Bahrain"),
new CountryInfo(50, "BD", "BGD", "Bangladesh"),
new CountryInfo(52, "BB", "BRB", "Barbados"),
new CountryInfo(112, "BY", "BLR", "Belarus"),
new CountryInfo(56, "BE", "BEL", "Belgium"),
new CountryInfo(84, "BZ", "BLZ", "Belize"),
new CountryInfo(204, "BJ", "BEN", "Benin"),
new CountryInfo(60, "BM", "BMU", "Bermuda"),
new CountryInfo(64, "BT", "BTN", "Bhutan"),
new CountryInfo(68, "BO", "BOL", "Bolivia"),
new CountryInfo(535, "BQ", "BES", "Bonaire, Sint Eustatius and Saba"),
new CountryInfo(70, "BA", "BIH", "Bosnia and Herzegovina"),
new CountryInfo(72, "BW", "BWA", "Botswana"),
new CountryInfo(74, "BV", "BVT", "Bouvet Island"),
new CountryInfo(76, "BR", "BRA", "Brazil"),
new CountryInfo(86, "IO", "IOT", "British Indian Ocean Territory"),
new CountryInfo(96, "BN", "BRN", "Brunei Darussalam"),
new CountryInfo(100, "BG", "BGR", "Bulgaria"),
new CountryInfo(854, "BF", "BFA", "Burkina Faso"),
new CountryInfo(108, "BI", "BDI", "Burundi"),
new CountryInfo(132, "CV", "CPV", "Cabo Verde"),
new CountryInfo(116, "KH", "KHM", "Cambodia"),
new CountryInfo(120, "CM", "CMR", "Cameroon"),
new CountryInfo(124, "CA", "CAN", "Canada"),
new CountryInfo(136, "KY", "CYM", "Cayman Islands"),
new CountryInfo(140, "CF", "CAF", "Central African Republic"),
new CountryInfo(148, "TD", "TCD", "Chad"),
new CountryInfo(152, "CL", "CHL", "Chile"),
new CountryInfo(156, "CN", "CHN", "China"),
new CountryInfo(162, "CX", "CXR", "Christmas Island"),
new CountryInfo(166, "CC", "CCK", "Cocos (Keeling) Islands"),
new CountryInfo(170, "CO", "COL", "Colombia"),
new CountryInfo(174, "KM", "COM", "Comoros"),
new CountryInfo(180, "CD", "COD", "Congo (the Democratic Republic of the)"),
new CountryInfo(178, "CG", "COG", "Congo"),
new CountryInfo(184, "CK", "COK", "Cook Islands"),
new CountryInfo(188, "CR", "CRI", "Costa Rica"),
new CountryInfo(191, "HR", "HRV", "Croatia"),
new CountryInfo(192, "CU", "CUB", "Cuba"),
new CountryInfo(531, "CW", "CUW", "Curaçao"),
new CountryInfo(196, "CY", "CYP", "Cyprus"),
new CountryInfo(203, "CZ", "CZE", "Czechia"),
new CountryInfo(384, "CI", "CIV", "Côte d'Ivoire"),
new CountryInfo(208, "DK", "DNK", "Denmark"),
new CountryInfo(262, "DJ", "DJI", "Djibouti"),
new CountryInfo(212, "DM", "DMA", "Dominica"),
new CountryInfo(214, "DO", "DOM", "Dominican Republic"),
new CountryInfo(218, "EC", "ECU", "Ecuador"),
new CountryInfo(818, "EG", "EGY", "Egypt"),
new CountryInfo(222, "SV", "SLV", "El Salvador"),
new CountryInfo(226, "GQ", "GNQ", "Equatorial Guinea"),
new CountryInfo(232, "ER", "ERI", "Eritrea"),
new CountryInfo(233, "EE", "EST", "Estonia"),
new CountryInfo(748, "SZ", "SWZ", "Eswatini"),
new CountryInfo(231, "ET", "ETH", "Ethiopia"),
new CountryInfo(238, "FK", "FLK", "Falkland Islands"),
new CountryInfo(234, "FO", "FRO", "Faroe Islands"),
new CountryInfo(242, "FJ", "FJI", "Fiji"),
new CountryInfo(246, "FI", "FIN", "Finland"),
new CountryInfo(250, "FR", "FRA", "France"),
new CountryInfo(254, "GF", "GUF", "French Guiana"),
new CountryInfo(258, "PF", "PYF", "French Polynesia"),
new CountryInfo(260, "TF", "ATF", "French Southern Territories"),
new CountryInfo(266, "GA", "GAB", "Gabon"),
new CountryInfo(270, "GM", "GMB", "Gambia"),
new CountryInfo(268, "GE", "GEO", "Georgia"),
new CountryInfo(276, "DE", "DEU", "Germany"),
new CountryInfo(288, "GH", "GHA", "Ghana"),
new CountryInfo(292, "GI", "GIB", "Gibraltar"),
new CountryInfo(300, "GR", "GRC", "Greece"),
new CountryInfo(304, "GL", "GRL", "Greenland"),
new CountryInfo(308, "GD", "GRD", "Grenada"),
new CountryInfo(312, "GP", "GLP", "Guadeloupe"),
new CountryInfo(316, "GU", "GUM", "Guam"),
new CountryInfo(320, "GT", "GTM", "Guatemala"),
new CountryInfo(831, "GG", "GGY", "Guernsey"),
new CountryInfo(324, "GN", "GIN", "Guinea"),
new CountryInfo(624, "GW", "GNB", "Guinea-Bissau"),
new CountryInfo(328, "GY", "GUY", "Guyana"),
new CountryInfo(332, "HT", "HTI", "Haiti"),
new CountryInfo(334, "HM", "HMD", "Heard Island and McDonald Islands"),
new CountryInfo(336, "VA", "VAT", "Holy See"),
new CountryInfo(340, "HN", "HND", "Honduras"),
new CountryInfo(344, "HK", "HKG", "Hong Kong"),
new CountryInfo(348, "HU", "HUN", "Hungary"),
new CountryInfo(352, "IS", "ISL", "Iceland"),
new CountryInfo(356, "IN", "IND", "India"),
new CountryInfo(360, "ID", "IDN", "Indonesia"),
new CountryInfo(364, "IR", "IRN", "Iran"),
new CountryInfo(368, "IQ", "IRQ", "Iraq"),
new CountryInfo(372, "IE", "IRL", "Ireland"),
new CountryInfo(833, "IM", "IMN", "Isle of Man"),
new CountryInfo(376, "IL", "ISR", "Israel"),
new CountryInfo(380, "IT", "ITA", "Italy"),
new CountryInfo(388, "JM", "JAM", "Jamaica"),
new CountryInfo(392, "JP", "JPN", "Japan"),
new CountryInfo(832, "JE", "JEY", "Jersey"),
new CountryInfo(400, "JO", "JOR", "Jordan"),
new CountryInfo(398, "KZ", "KAZ", "Kazakhstan"),
new CountryInfo(404, "KE", "KEN", "Kenya"),
new CountryInfo(296, "KI", "KIR", "Kiribati"),
new CountryInfo(408, "KP", "PRK", "Korea (the Democratic People's Republic of)"),
new CountryInfo(410, "KR", "KOR", "Korea"),
new CountryInfo(414, "KW", "KWT", "Kuwait"),
new CountryInfo(417, "KG", "KGZ", "Kyrgyzstan"),
new CountryInfo(418, "LA", "LAO", "Lao People's Democratic Republic"),
new CountryInfo(428, "LV", "LVA", "Latvia"),
new CountryInfo(422, "LB", "LBN", "Lebanon"),
new CountryInfo(426, "LS", "LSO", "Lesotho"),
new CountryInfo(430, "LR", "LBR", "Liberia"),
new CountryInfo(434, "LY", "LBY", "Libya"),
new CountryInfo(438, "LI", "LIE", "Liechtenstein"),
new CountryInfo(440, "LT", "LTU", "Lithuania"),
new CountryInfo(442, "LU", "LUX", "Luxembourg"),
new CountryInfo(446, "MO", "MAC", "Macao"),
new CountryInfo(807, "MK", "MKD", "Macedonia"),
new CountryInfo(450, "MG", "MDG", "Madagascar"),
new CountryInfo(454, "MW", "MWI", "Malawi"),
new CountryInfo(458, "MY", "MYS", "Malaysia"),
new CountryInfo(462, "MV", "MDV", "Maldives"),
new CountryInfo(466, "ML", "MLI", "Mali"),
new CountryInfo(470, "MT", "MLT", "Malta"),
new CountryInfo(584, "MH", "MHL", "Marshall Islands"),
new CountryInfo(474, "MQ", "MTQ", "Martinique"),
new CountryInfo(478, "MR", "MRT", "Mauritania"),
new CountryInfo(480, "MU", "MUS", "Mauritius"),
new CountryInfo(175, "YT", "MYT", "Mayotte"),
new CountryInfo(484, "MX", "MEX", "Mexico"),
new CountryInfo(583, "FM", "FSM", "Micronesia"),
new CountryInfo(498, "MD", "MDA", "Moldova"),
new CountryInfo(492, "MC", "MCO", "Monaco"),
new CountryInfo(496, "MN", "MNG", "Mongolia"),
new CountryInfo(499, "ME", "MNE", "Montenegro"),
new CountryInfo(500, "MS", "MSR", "Montserrat"),
new CountryInfo(504, "MA", "MAR", "Morocco"),
new CountryInfo(508, "MZ", "MOZ", "Mozambique"),
new CountryInfo(104, "MM", "MMR", "Myanmar"),
new CountryInfo(516, "NA", "NAM", "Namibia"),
new CountryInfo(520, "NR", "NRU", "Nauru"),
new CountryInfo(524, "NP", "NPL", "Nepal"),
new CountryInfo(528, "NL", "NLD", "Netherlands"),
new CountryInfo(540, "NC", "NCL", "New Caledonia"),
new CountryInfo(554, "NZ", "NZL", "New Zealand"),
new CountryInfo(558, "NI", "NIC", "Nicaragua"),
new CountryInfo(562, "NE", "NER", "Niger"),
new CountryInfo(566, "NG", "NGA", "Nigeria"),
new CountryInfo(570, "NU", "NIU", "Niue"),
new CountryInfo(574, "NF", "NFK", "Norfolk Island"),
new CountryInfo(580, "MP", "MNP", "Northern Mariana Islands"),
new CountryInfo(578, "NO", "NOR", "Norway"),
new CountryInfo(512, "OM", "OMN", "Oman"),
new CountryInfo(586, "PK", "PAK", "Pakistan"),
new CountryInfo(585, "PW", "PLW", "Palau"),
new CountryInfo(275, "PS", "PSE", "Palestine, State of"),
new CountryInfo(591, "PA", "PAN", "Panama"),
new CountryInfo(598, "PG", "PNG", "Papua New Guinea"),
new CountryInfo(600, "PY", "PRY", "Paraguay"),
new CountryInfo(604, "PE", "PER", "Peru"),
new CountryInfo(608, "PH", "PHL", "Philippines"),
new CountryInfo(612, "PN", "PCN", "Pitcairn"),
new CountryInfo(616, "PL", "POL", "Poland"),
new CountryInfo(620, "PT", "PRT", "Portugal"),
new CountryInfo(630, "PR", "PRI", "Puerto Rico"),
new CountryInfo(634, "QA", "QAT", "Qatar"),
new CountryInfo(642, "RO", "ROU", "Romania"),
new CountryInfo(643, "RU", "RUS", "Russian Federation"),
new CountryInfo(646, "RW", "RWA", "Rwanda"),
new CountryInfo(638, "RE", "REU", "Réunion"),
new CountryInfo(652, "BL", "BLM", "Saint Barthélemy"),
new CountryInfo(654, "SH", "SHN", "Saint Helena, Ascension and Tristan da Cunha"),
new CountryInfo(659, "KN", "KNA", "Saint Kitts and Nevis"),
new CountryInfo(662, "LC", "LCA", "Saint Lucia"),
new CountryInfo(663, "MF", "MAF", "Saint Martin"),
new CountryInfo(666, "PM", "SPM", "Saint Pierre and Miquelon"),
new CountryInfo(670, "VC", "VCT", "Saint Vincent and the Grenadines"),
new CountryInfo(882, "WS", "WSM", "Samoa"),
new CountryInfo(674, "SM", "SMR", "San Marino"),
new CountryInfo(678, "ST", "STP", "Sao Tome and Principe"),
new CountryInfo(682, "SA", "SAU", "Saudi Arabia"),
new CountryInfo(686, "SN", "SEN", "Senegal"),
new CountryInfo(688, "RS", "SRB", "Serbia"),
new CountryInfo(690, "SC", "SYC", "Seychelles"),
new CountryInfo(694, "SL", "SLE", "Sierra Leone"),
new CountryInfo(702, "SG", "SGP", "Singapore"),
new CountryInfo(534, "SX", "SXM", "Sint Maarten (Dutch part)"),
new CountryInfo(703, "SK", "SVK", "Slovakia"),
new CountryInfo(705, "SI", "SVN", "Slovenia"),
new CountryInfo(90, "SB", "SLB", "Solomon Islands"),
new CountryInfo(706, "SO", "SOM", "Somalia"),
new CountryInfo(710, "ZA", "ZAF", "South Africa"),
new CountryInfo(239, "GS", "SGS", "South Georgia and the South Sandwich Islands"),
new CountryInfo(728, "SS", "SSD", "South Sudan"),
new CountryInfo(724, "ES", "ESP", "Spain"),
new CountryInfo(144, "LK", "LKA", "Sri Lanka"),
new CountryInfo(729, "SD", "SDN", "Sudan (the)"),
new CountryInfo(740, "SR", "SUR", "Suriname"),
new CountryInfo(744, "SJ", "SJM", "Svalbard and Jan Mayen"),
new CountryInfo(752, "SE", "SWE", "Sweden"),
new CountryInfo(756, "CH", "CHE", "Switzerland"),
new CountryInfo(760, "SY", "SYR", "Syrian Arab Republic"),
new CountryInfo(158, "TW", "TWN", "Taiwan (Province of China)"),
new CountryInfo(762, "TJ", "TJK", "Tajikistan"),
new CountryInfo(834, "TZ", "TZA", "Tanzania, United Republic of"),
new CountryInfo(764, "TH", "THA", "Thailand"),
new CountryInfo(626, "TL", "TLS", "Timor-Leste"),
new CountryInfo(768, "TG", "TGO", "Togo"),
new CountryInfo(772, "TK", "TKL", "Tokelau"),
new CountryInfo(776, "TO", "TON", "Tonga"),
new CountryInfo(780, "TT", "TTO", "Trinidad and Tobago"),
new CountryInfo(788, "TN", "TUN", "Tunisia"),
new CountryInfo(792, "TR", "TUR", "Turkey"),
new CountryInfo(795, "TM", "TKM", "Turkmenistan"),
new CountryInfo(796, "TC", "TCA", "Turks and Caicos Islands"),
new CountryInfo(798, "TV", "TUV", "Tuvalu"),
new CountryInfo(800, "UG", "UGA", "Uganda"),
new CountryInfo(804, "UA", "UKR", "Ukraine"),
new CountryInfo(784, "AE", "ARE", "United Arab Emirates"),
new CountryInfo(826, "GB", "GBR", "United Kingdom"),
new CountryInfo(581, "UM", "UMI", "United States Minor Outlying Islands"),
new CountryInfo(840, "US", "USA", "United States of America"),
new CountryInfo(858, "UY", "URY", "Uruguay"),
new CountryInfo(860, "UZ", "UZB", "Uzbekistan"),
new CountryInfo(548, "VU", "VUT", "Vanuatu"),
new CountryInfo(862, "VE", "VEN", "Venezuela"),
new CountryInfo(704, "VN", "VNM", "Viet Nam"),
new CountryInfo(92, "VG", "VGB", "Virgin Islands (British)"),
new CountryInfo(850, "VI", "VIR", "Virgin Islands (U.S.)"),
new CountryInfo(876, "WF", "WLF", "Wallis and Futuna"),
new CountryInfo(732, "EH", "ESH", "Western Sahara"),
new CountryInfo(887, "YE", "YEM", "Yemen"),
new CountryInfo(894, "ZM", "ZMB", "Zambia"),
new CountryInfo(716, "ZW", "ZWE", "Zimbabwe"),
new CountryInfo(248, "AX", "ALA", "Åland Islands"),
            };

            return countryModels;
        }
    }
}