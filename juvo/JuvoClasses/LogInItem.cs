using Newtonsoft.Json;

namespace juvo.JuvoClasses
{
    public class LogInItem
    {
        public string Id { get; set; }

        [JsonProperty(PropertyName = "email")]
        public string Email { get; set; }

        [JsonProperty(PropertyName = "password")]
        public string Password { get; set; }
    }

    public class LogInResponse
    {
        [JsonProperty(PropertyName = "username")]
        public string Email { get; set; }

        [JsonProperty(PropertyName = "token")]
        public string Token { get; set; }

        [JsonProperty(PropertyName = "userid")]
        public string UserId { get; set; }
    }

    public class LogInItemWrapper : Java.Lang.Object
    {
        public LogInItemWrapper(LogInItem item)
        {
            LogInItem = item;
        }

        public LogInItem LogInItem { get; private set; }
    }
}