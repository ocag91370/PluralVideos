using Newtonsoft.Json;

namespace PluralVideos.Services.Video
{
    public class Header
    {
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "title")]
        public string Title { get; set; }

        [JsonProperty(PropertyName = "hasTranscript")]
        public bool HasTranscript { get; set; }
    }
}
