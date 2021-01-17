using Newtonsoft.Json;
using System.Collections.Generic;

namespace PluralVideos.Services.Video
{
    public class ClipUrls
    {
        [JsonProperty(PropertyName = "url")]
        public string Url { get; set; }

        [JsonProperty(PropertyName = "rankedOptions")]
        public List<UrlOption> RankedOptions { get; set; }
    }

    public class UrlOption
    {
        [JsonProperty(PropertyName = "cdn")]
        public string Cdn { get; set; }

        [JsonProperty(PropertyName = "url")]
        public string Url { get; set; }
    }
}
