using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text.Json.Serialization;
using System.Xml.Linq;
using UAParser;

namespace servicio_social_api.Controllers
{
    [ApiController]
    [Route("api/")]
    public class CommentsController : ControllerBase
    {

        private readonly ILogger<CommentsController> _logger;

        public CommentsController(ILogger<CommentsController> logger)
        {
            _logger = logger;
        }

        [HttpGet("GetComments")]        
        public async Task<IActionResult> Post()
        {
            string userAgent = Request.Headers["User-Agent"];
            var uaParser = Parser.GetDefault();
            ClientInfo clientInfo = uaParser.Parse(userAgent);
            string browser = clientInfo.UA.Family;
            string os = clientInfo.OS.Family;
            string referrer = Request.Headers["Referer"];
            string ipAddress = Request.HttpContext.Connection.RemoteIpAddress!.ToString();
            string countryCode = await GetCountryCodeFromIp(ipAddress);
            return Ok(countryCode);
        }
        private async Task<string> GetCountryCodeFromIp(string ipAddress)
        {
            string countryCode = "";
            string json = "";
            try
            {
                using (var client = new HttpClient())
                {
                    var response = await client.GetAsync("https://ipapi.co/" + ipAddress + "/json/");
                    if (response.IsSuccessStatusCode)
                    {
                        json = await response.Content.ReadAsStringAsync();
                        IpApiCoResponse ipApiCoResponse = JsonConvert.DeserializeObject<IpApiCoResponse>(json)!;
                        countryCode = ipApiCoResponse.CountryCode!;
                    }
                }
            }
            catch (HttpRequestException e)
            {
                _logger.LogError(e.Message);
            }
            return json;
        }

        private class IpApiCoResponse
        {
            [JsonPropertyName("ip")]
            public string? Ip { get; set; }

            [JsonPropertyName("network")]
            public string? Network { get; set; }

            [JsonPropertyName("version")]
            public string? Version { get; set; }

            [JsonPropertyName("city")]
            public string? City { get; set; }

            [JsonPropertyName("region")]
            public string? Region { get; set; }

            [JsonPropertyName("region_code")]
            public string? RegionCode { get; set; }

            [JsonPropertyName("country")]
            public string? Country { get; set; }

            [JsonPropertyName("country_name")]
            public string? CountryName { get; set; }

            [JsonPropertyName("country_code")]
            public string? CountryCode { get; set; }

            [JsonPropertyName("country_code_iso3")]
            public string? CountryCodeIso3 { get; set; }

            [JsonPropertyName("country_capital")]
            public string? CountryCapital { get; set; }

            [JsonPropertyName("country_tld")]
            public string? CountryTld { get; set; }

            [JsonPropertyName("continent_code")]
            public string? ContinentCode { get; set; }

            [JsonPropertyName("in_eu")]
            public bool? InEu { get; set; }

            [JsonPropertyName("postal")]
            public string? Postal { get; set; }

            [JsonPropertyName("latitude")]
            public double? Latitude { get; set; }

            [JsonPropertyName("longitude")]
            public double? Longitude { get; set; }

            [JsonPropertyName("timezone")]
            public string? Timezone { get; set; }

            [JsonPropertyName("utc_offset")]
            public string? UtcOffset { get; set; }

            [JsonPropertyName("country_calling_code")]
            public string? CountryCallingCode { get; set; }

            [JsonPropertyName("currency")]
            public string? Currency { get; set; }

            [JsonPropertyName("currency_name")]
            public string? CurrencyName { get; set; }

            [JsonPropertyName("languages")]
            public string? Languages { get; set; }

            [JsonPropertyName("country_area")]
            public double? CountryArea { get; set; }

            [JsonPropertyName("country_population")]
            public int? CountryPopulation { get; set; }

            [JsonPropertyName("asn")]
            public string? Asn { get; set; }

            [JsonPropertyName("org")]
            public string? Org { get; set; }
        }
    }
}