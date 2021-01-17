using Newtonsoft.Json;

namespace PluralVideos.Services.Video
{
    public class Clip
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "title")]
        public string Title { get; set; }

        [JsonProperty(PropertyName = "index")]
        public int Index { get; set; }

        [JsonProperty(PropertyName = "durationInMilliseconds")]
        public int DurationInMilliseconds { get; set; }

        [JsonProperty(PropertyName = "supportsStandard")]
        public bool SupportsStandard { get; set; }

        [JsonProperty(PropertyName = "supportsWideScreen")]
        public bool SupportsWidescreen { get; set; }
    }
}
