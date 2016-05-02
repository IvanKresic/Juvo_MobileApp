using Newtonsoft.Json;

namespace juvo.JuvoModel
{
    public class Response
    {
        public int Id { get; set; }

        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "time")]
        public string Time { get; set; }

    }
}