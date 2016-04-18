using Newtonsoft.Json;

namespace juvo.JuvoClasses
{
    public class DangerEvents
    {
        public string Id { get; set; }

        [JsonProperty(PropertyName = "primaryid")]
        public int PrimaryID { get; set; }

        [JsonProperty(PropertyName = "deviceid")]
        public int DeviceId { get; set; }

        [JsonProperty(PropertyName = "happenedat")]
        public string HappenedAt { get; set; }
    }

    public class HistoryItemWrapper : Java.Lang.Object
    {
        public HistoryItemWrapper(DangerEvents item)
        {
            HistoryItem = item;
        }

        public DangerEvents HistoryItem { get; private set; }
    }    
}