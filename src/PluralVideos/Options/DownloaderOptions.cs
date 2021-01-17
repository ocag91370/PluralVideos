using CommandLine;

namespace PluralVideos.Options
{
    [Verb("download", HelpText = "Downloads courses from pluralsight")]
    public class DownloaderOptions
    {
        [Option("out", Required = true, HelpText = "Output folder path")]
        public string OutputPath { get; set; }

        [Option("course", Required = true, HelpText = "Course to download")]
        public string CourseId { get; set; }

        [Option("module", HelpText = "Video clip to download")]
        public string ModuleId { get; set; }

        [Option("clip", HelpText = "Video clip to download")]
        public string ClipId { get; set; }

        [Option("list", HelpText = "List course without downloading")]
        public bool ListCourse { get; set; }

        [Option("timeout", Default = 15, HelpText = "Timeout period for video download in seconds")]
        public int Timeout { get; set; }

        public bool ListClip
            => !string.IsNullOrEmpty(ClipId) && ListCourse;

        public bool ListModule
            => !string.IsNullOrEmpty(ModuleId) && ListCourse;

        public bool DownloadClip
            => !string.IsNullOrEmpty(ClipId) && !ListCourse;

        public bool DownloadModule
            => !string.IsNullOrEmpty(ModuleId) && !ListCourse;
    }
}
