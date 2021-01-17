using PluralVideos.Services.Decryptor;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace PluralVideos.Helpers
{
    public static class DecryptFileHelper
    {
        public static DirectoryInfo GetCourseFolder(string coursesFolder, string courseName)
        {
            var di = new DirectoryInfo($@"{CoursePath(coursesFolder, courseName)}");
            return di;
        }

        public static DirectoryInfo GetModuleFolder(string coursesFolder, Module module)
        {
            string s = $"{module.Name}|{module.AuthorHandle}";
            using var md5 = MD5.Create();
            var moduleHash = Convert.ToBase64String(md5.ComputeHash(Encoding.UTF8.GetBytes(s))).Replace('/', '_');

            var modulePath = $@"{CoursePath(coursesFolder, module.CourseName)}\{moduleHash}";

            return new DirectoryInfo(modulePath);
        }

        public static FileStream CreateVideo(string outputFolder, string courseTitle, Module module, Clip clip)
        {
            var decryptedFolder = $@"{outputFolder}\{courseTitle.RemoveInvalidCharacter()}\{module.ModuleIndex + 1:00} - {module.Title.RemoveInvalidCharacter()}";
            var fi = new FileInfo($@"{decryptedFolder}\{clip.ClipIndex + 1:00} - {clip.Title.RemoveInvalidCharacter()}.mp4");
            if (!fi.Directory.Exists)
                fi.Directory.Create();
            return fi.Create();
        }

        public static FileStream CreateVideoTranscript(string outputFolder, string courseTitle, Module module, Clip clip)
        {
            var decryptedFolder = $@"{outputFolder}\{courseTitle.RemoveInvalidCharacter()}\{module.ModuleIndex + 1:00} - {module.Title.RemoveInvalidCharacter()}";
            var fi = new FileInfo($@"{decryptedFolder}\{clip.ClipIndex + 1:00} - {clip.Title.RemoveInvalidCharacter()}.srt");
            if (!fi.Directory.Exists)
                fi.Directory.Create();
            return fi.Create();
        }

        private static string CoursePath(string coursesPath, string courseName) => $@"{coursesPath}\{courseName}";

        private static string RemoveInvalidCharacter(this string value)
        {
            var illegalInFileName = new Regex(@"[\\/:*?""<>|]");
            return illegalInFileName.Replace(value.Trim(), "");
        }
    }
}
