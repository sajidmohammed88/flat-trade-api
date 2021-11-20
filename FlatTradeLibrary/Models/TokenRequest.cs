using Newtonsoft.Json;

namespace FlatTradeLibrary.Models
{
    public class TokenRequest
    {
        [JsonProperty("api_secret")]
        public string ApiSecret { get; set; }
        [JsonProperty("request_code")]
        public string RequestCode { get; set; }
        [JsonProperty("api_key")]
        public string ApiKey { get; set; }
    }
}
