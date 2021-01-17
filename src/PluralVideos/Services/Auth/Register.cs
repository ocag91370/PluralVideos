using Newtonsoft.Json;
using System;

namespace PluralVideos.Services.Auth
{
    public class Register
    {
        public string DeviceModel => "Windows Desktop";

        public string DeviceName { get; set; }
    }

    public class RegisterResponse
    {
        [JsonProperty(PropertyName = "deviceId")]
        public string DeviceId { get; set; }

        [JsonProperty(PropertyName = "pin")]
        public string Pin { get; set; }

        [JsonProperty(PropertyName = "refreshToken")]
        public string RefreshToken { get; set; }

        [JsonProperty(PropertyName = "validUntil")]
        public DateTimeOffset ValidUntil { get; set; }

        [JsonProperty(PropertyName = "serverTime")]
        public DateTimeOffset ServerTime { get; set; }

        [JsonProperty(PropertyName = "authDeviceUrl")]
        public string AuthDeviceUrl { get; set; }
    }
}
