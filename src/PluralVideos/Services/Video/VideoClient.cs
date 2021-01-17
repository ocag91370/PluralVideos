using PluralVideos.Helpers;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace PluralVideos.Services.Video
{
    public class VideoClient : BaseClient
    {
        public VideoClient(Func<bool, Task<string>> getAccessToken, HttpClient httpClientFactory)
            : base(getAccessToken, httpClientFactory) { }

        public async Task<ApiResponse<Course>> GetCourse(string courseName)
        {
            var course = DownloadFileHelper.ReadCourse(courseName);
            if (course == null)
            {
                var response = await GetHttp<Course>($"library/courses/{courseName}");
                if (response.Success)
                    DownloadFileHelper.WriteCourse(response.Data);
                return response;
            }

            return new ApiResponse<Course>
            {
                Data = course
            };
        }

        public async Task<bool?> HasCourseAccess(string courseName)
        {
            var response = await GetHttp<CourseAccess>($"user/courses/{courseName}/access", requiresAuthentication: true);
            return response.Success ? response.Data?.MayDownload : new bool?();
        }
    }
}
