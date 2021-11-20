using Newtonsoft.Json;

namespace FlatTradeLibrary.Models
{
    public class RedirectResponse : BaseMessage
    {
        [JsonProperty("RedirectURL")]
        public string RedirectUrl { get; set; }
    }
}
