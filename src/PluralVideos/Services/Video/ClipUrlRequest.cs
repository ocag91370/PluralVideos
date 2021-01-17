namespace PluralVideos.Services.Video
{
    public class ClipUrlRequest
    {
        public ClipUrlRequest(bool supportsWidescreen)
        {
            AspectRatio = supportsWidescreen ? "widescreen" : "standard";
        }

        public string CourseId { get; set; }

        public string ClipId { get; set; }

        public string AspectRatio { get; }
    }
}
