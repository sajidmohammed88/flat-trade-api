using Newtonsoft.Json;

namespace FlatTradeLibrary.Models
{
    public class Request
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        [JsonProperty("PAN_DOB")]
        public string Pan { get; set; }
        [JsonProperty("APIKey")]
        public string ApiKey { get; set; }
        [JsonProperty("Sid")]
        public string SessionId { get; set; }
        [JsonIgnore]
        public string ApiSecret { get; set; }
    }
}
