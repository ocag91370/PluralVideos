using Newtonsoft.Json;
using System;

namespace PluralVideos.Services.Auth
{
    public class User
    {
        public DeviceInfo DeviceInfo { get; set; }

        [JsonProperty(PropertyName = "token")]
        public string Jwt { get; set; }

        [JsonProperty(PropertyName = "expiration")]
        public DateTimeOffset Expiration { get; set; }

        [JsonProperty(PropertyName = "userHandle")]
        public string UserHandle { get; set; }
    }
}
