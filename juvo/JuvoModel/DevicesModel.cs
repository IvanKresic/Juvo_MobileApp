using System.Collections.Generic;
using Newtonsoft.Json;

namespace juvo.JuvoModel
{
    public class DevicesModel
    {
        [JsonProperty(PropertyName = "deviceId")]
        public int DevicesID { get; set; }

        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "userId")]
        public int UserId { get; set; }

        [JsonProperty(PropertyName = "DangerEvents")]
        public List<DangerEventsModel> DangerEvents { get; set; }

    }
}