using Newtonsoft.Json;

namespace FlatTradeLibrary.Models
{
    public class BaseMessage
    {
        [JsonProperty("stat")]
        public string Status { get; set; }
        [JsonProperty("emsg")]
        public string Message { get; set; }
    }
}
