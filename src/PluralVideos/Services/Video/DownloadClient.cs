using PluralVideos.Helpers;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace PluralVideos.Services.Video
{
    public class DownloadClient : BaseClient
    {
        private readonly string outputPath;
        private readonly Header course;
        public int ModuleId { get; }
        public string ModuleTitle { get; }
        public Clip Clip { get; }

        public DownloadClient(string outputPath, Header course, int moduleId, string moduleTitle, Clip clip, HttpClient httpClientFactory, Func<bool, Task<string>> getAccessToken = null)
            : base(getAccessToken, httpClientFactory)
        {
            this.outputPath = outputPath ?? throw new ArgumentNullException(nameof(outputPath));
            this.course = course ?? throw new ArgumentNullException(nameof(course));
            ModuleId = moduleId;
            ModuleTitle = moduleTitle ?? throw new ArgumentNullException(nameof(moduleTitle));
            Clip = clip ?? throw new ArgumentNullException(nameof(clip));
        }

        public async Task<bool> Download()
        {
            var clipsResponse = await GetClipUrls();
            if (!clipsResponse.Success)
                return false;

            var completed = false;
            foreach (var item in clipsResponse.Data.RankedOptions)
            {
                var head = await HeadHttp(item.Url);
                if (!head.Success)
                    continue;

                var filePath = DownloadFileHelper.GetVideoPath(outputPath, course.Title, ModuleId, ModuleTitle, Clip);
                using var fs = DownloadFileHelper.CreateFile(filePath);

                var response = await GetFile(item.Url);
                if (response.Success)
                {
                    var buffer = new byte[0x2000];
                    int read;
                    while ((read = response.Data.Read(buffer, 0, buffer.Length)) != 0)
                        fs.Write(buffer, 0, read);

                    completed = true;
                    break;
                }
            }

            return completed;
        }

        private async Task<ApiResponse<ClipUrls>> GetClipUrls() =>
            await PostHttp<ClipUrls>($"library/videos/offline", new ClipUrlRequest(Clip.SupportsWidescreen) { ClipId = Clip.Id, CourseId = course.Id }, requiresAuthentication: true);

    }
}
