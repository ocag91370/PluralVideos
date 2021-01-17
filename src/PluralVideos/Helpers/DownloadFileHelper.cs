using PluralVideos.Services.Auth;
using PluralVideos.Services.Video;
using System;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace PluralVideos.Helpers
{
    public static class DownloadFileHelper
    {
        private static readonly JsonSerializerOptions options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = true
        };

        public static string GetVideoPath(string outputFolder, string courseTitle, int moduleId, string moduleTitle, Clip clip) =>
            $@"{outputFolder}\{courseTitle.RemoveInvalidCharacter()}\{moduleId + 1:00} - {moduleTitle.RemoveInvalidCharacter()}\{clip.Index}. {clip.Title.RemoveInvalidCharacter()}.mp4";

        public static User ReadUser()
        {
            var file = new FileInfo(UserPath);
            if (!file.Exists)
                return null;
            var json = File.ReadAllText(UserPath);
            return JsonSerializer.Deserialize<User>(json, options);
        }

        public static void DeleteUser()
        {
            var file = new FileInfo(UserPath);
            if (file.Exists)
                file.Delete();
        }

        public static void WriteUser(User user)
        {
            using var fs = CreateFile(UserPath);
            var json = JsonSerializer.Serialize(user, options);
            fs.Write(Encoding.UTF8.GetBytes(json));
        }

        public static Course ReadCourse(string courseName)
        {
            var courseFile = $@"{CoursePath}\{courseName}.json";
            var file = new FileInfo(courseFile);
            if (!file.Exists)
                return null;
            var json = File.ReadAllText(courseFile);
            return JsonSerializer.Deserialize<Course>(json, options);
        }

        public static void WriteCourse(Course course)
        {
            using var fs = CreateFile(@$"{CoursePath}\{course.Header.Name}.json");
            var json = JsonSerializer.Serialize(course, options);
            fs.Write(Encoding.UTF8.GetBytes(json));
        }

        public static FileStream CreateFile(string filePath)
        {
            var file = new FileInfo(filePath);
            if (!file.Directory.Exists)
                file.Directory.Create();
            return file.Exists ? file.OpenWrite() : file.Create();
        }

        private static string RemoveInvalidCharacter(this string value)
        {
            var illegalInFileName = new Regex(@"[\\/:*?""<>|]");
            return illegalInFileName.Replace(value.Trim(), "");
        }

        private static string UserPath =>
            $@"{LocalPath}\user.json";

        private static string CoursePath =>
            $@"{LocalPath}\courses";

        private static string LocalPath =>
            $@"{Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)}\PluralVideos";
    }
}
