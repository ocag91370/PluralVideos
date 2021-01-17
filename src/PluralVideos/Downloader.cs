using PluralVideos.Options;
using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using PluralVideos.Helpers;
using PluralVideos.Extensions;
using PluralVideos.Services.Video;
using PluralVideos.Services;

namespace PluralVideos
{
    public class Downloader
    {
        private readonly DownloaderOptions options;

        private readonly TaskQueue queue;

        private readonly PluralSightApi api;

        private readonly HttpClient httpClient = new HttpClient();

        public Downloader(DownloaderOptions options)
        {
            this.options = options;
            api = new PluralSightApi(options.Timeout);
            queue = new TaskQueue();
            queue.ProcessCompleteEvent += ClipDownloaded;
        }

        public async Task Download()
        {
            if (options.ListClip || options.ListModule)
                Utils.WriteRedText("--list cannot be used with --clip or --module");

            var course = await GetCourseAsync(options.CourseId, list: false);
            Utils.WriteYellowText($"Downloading from course'{course.Header.Title}' started ...");

            if (options.DownloadClip)
            {
                var (clip, index, title) = course.GetClip(options.ClipId);
                GetClipAsync(clip, course.Header, index, title, list: false);
            }
            else if (options.DownloadModule)
            {
                var (module, index) = course.Modules.WithIndex()
                    .FirstOrDefault(i => i.item.Id == options.ModuleId);

                GetModuleAsync(course.Header, module, index, list: false);
            }
            else
            {
                foreach (var (module, index) in course.Modules.WithIndex())
                    GetModuleAsync(course.Header, module, index, options.ListCourse);
            }

            Utils.WriteGreenText($"\tDownloading has started ...");

            await queue.Execute();

            Utils.WriteYellowText($"Download complete");
        }

        private async Task<Course> GetCourseAsync(string courseName, bool list)
        {
            var courseResponse = await api.Video.GetCourse(courseName);
            if (!courseResponse.Success)
                throw new Exception($"Course was not found. Error: {courseResponse.ResponseBody}");

            var hasAccess = await api.Video.HasCourseAccess(courseResponse.Data.Header.Id);
            var noAccess = (!hasAccess.HasValue || !hasAccess.Value);
            if (noAccess  && !list)
                throw new Exception("You do not have permission to download this course");
            else if (!noAccess&& list)
                Utils.WriteRedText("Warning: You do not have permission to download this course");

            return courseResponse.Data;
        }

        private void GetModuleAsync(Header course, Module module, int index, bool list)
        {
            if (module == null)
                throw new Exception("The module was not found. Check the module and Try again.");

            if (list)
            {
                Utils.WriteGreenText($"\t{index + 1:00} - {module.Title}", newLine: false);
                Utils.WriteBlueText($"  --  {module.Id}");
            }

            foreach (var clip in module.Clips)
                GetClipAsync(clip, course, index, module.Title, list);
        }

        private void GetClipAsync(Clip clip, Header course, int moduleId, string moduleTitle, bool list)
        {
            if (clip == null)
                throw new Exception("The clip was not found. Check the clip and Try again.");

            if (list)
            {
                Utils.WriteText($"\t\t{clip.Index + 1:00} - {clip.Title}", newLine: false);
                Utils.WriteCyanText($"  --  {clip.Id}");
                return;
            }

            var client = new DownloadClient(options.OutputPath, course, moduleId, moduleTitle, clip, httpClient, api.GetAccessToken);
            queue.Enqueue(client);
        }

        private void ClipDownloaded(object sender, DownloadEventArgs e)
        {
            Utils.WriteGreenText($"\n\t{e.ModuleId + 1:00} - {e.ModuleTitle}");
            if (e.Succeeded)
                Utils.WriteText($"\t\t{e.ClipId + 1:00} - {e.ClipTitle}  --  downloaded");
            else
                Utils.WriteRedText($"\t\t{e.ClipId + 1:00} - {e.ClipTitle} --  Download failed. will retry again.");
        }
    }
}
