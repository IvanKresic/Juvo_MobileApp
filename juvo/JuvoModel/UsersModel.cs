using System.Collections.Generic;
using Newtonsoft.Json;

namespace juvo.JuvoModel
{
    public class UsersModel
    {
        [JsonProperty(PropertyName = "userId")]
        public int UsersID { get; set; }

        [JsonProperty(PropertyName = "userName")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "email")]
        public string Email { get; set; }

        [JsonProperty(PropertyName = "timeZone")]
        public string TimeZone { get; set; }

        [JsonProperty(PropertyName = "activated")]
        public bool Activated { get; set; }

        [JsonProperty(PropertyName = "devices")]
        public List<DevicesModel> devices { get; set; }
    }

}