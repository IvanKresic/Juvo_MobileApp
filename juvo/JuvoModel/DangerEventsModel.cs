using Newtonsoft.Json;

namespace juvo.JuvoModel
{
     public class DangerEventsModel
    {
        [JsonProperty(PropertyName = "primaryid")]
        public int PrimaryID { get; set; }

        [JsonProperty(PropertyName = "deviceId")]
        public int DeviceId { get; set; }

        [JsonProperty(PropertyName = "HappenedAt")]
        public string HappenedAt { get; set; }
    }
}