using Newtonsoft.Json;
using System.Collections.Generic;

namespace PluralVideos.Services.Video
{
    public partial class Module
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "title")]
        public string Title { get; set; }

        [JsonProperty(PropertyName = "authorHandle")]
        public string AuthorHandle { get; set; }

        [JsonProperty(PropertyName = "description")]
        public string Description { get; set; }

        [JsonProperty(PropertyName = "durationInMilliseconds")]
        public int DurationInMilliseconds { get; set; }

        [JsonProperty(PropertyName = "hasLearningChecks")]
        public bool HasLearningChecks { get; set; }

        [JsonProperty(PropertyName = "learningChecksCount")]
        public int LearningChecksCount { get; set; }

        [JsonProperty(PropertyName = "clips")]
        public virtual ICollection<Clip> Clips { get; set; }
    }
}
