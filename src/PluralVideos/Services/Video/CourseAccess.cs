using Newtonsoft.Json;

namespace PluralVideos.Services.Video
{
    public class CourseAccess
    {
        [JsonProperty(PropertyName = "mayDownload")]
        public bool? MayDownload { get; set; }
    }
}
