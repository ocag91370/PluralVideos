using PluralVideos.Encryption;
using PluralVideos.Helpers;
using PluralVideos.Options;
using PluralVideos.Services.Decryptor;
using System;
using System.IO;
using System.Linq;

namespace PluralVideos
{
    public class Decryptor
    {
        private readonly Repository repository;
        private readonly DecryptorOptions options;

        public Decryptor(DecryptorOptions options)
        {
            repository = new Repository(options);
            this.options = options;
        }

        public void DecryptCourses()
        {
            var courses = repository.GetCourses();

            if (string.IsNullOrWhiteSpace(options.CourseTag))
            {
                courses.ForEach(course => DecryptCourse(course));
            }
            else
            {
                var course = courses.FirstOrDefault(o => o.UrlSlug.Contains(options.CourseTag));
                DecryptCourse(course);
            }
        }

        public void DecryptCourse(Course course)
        {
            if (course == null)
            {
                Utils.WriteRedText($"\t{course.Name} Folder does not exist");
                return;
            }

            Utils.WriteYellowText($"Decrypting '{course.Title} started ...");
            var courseFolder = DecryptFileHelper.GetCourseFolder(options.CoursesPath, course.Name);
            if (!courseFolder.Exists)
                Utils.WriteRedText($"\t{course.Name} Folder does not exist");
            {
                var modules = repository.GetModules(course.Name);
                modules.ForEach(module => CreateModule(module, course));
                if (options.RemoveFolderAfterDecryption)
                    RemoveCourse(course);
                Utils.WriteYellowText($"Decrypting '{course.Title}' complete");
            }
        }

        private void CreateModule(Module module, Course course)
        {
            Utils.WriteGreenText($"\t{module.ModuleIndex + 1:00} - {module.Title}");
            var moduleFolder = DecryptFileHelper.GetModuleFolder(options.CoursesPath, module);
            if (!moduleFolder.Exists)
                Utils.WriteRedText($"Module Folder does not exist");
            else
            {
                var clips = repository.GetClips(module.Id);
                clips.ForEach(c => CreateVideoClip(c, module, moduleFolder, course.Title));
            }
        }

        private void CreateVideoClip(Clip clip, Module module, DirectoryInfo moduleFolder, string courseTitle)
        {
            Utils.WriteText($"\t\t{clip.ClipIndex + 1:00} - {clip.Title}");
            var file = moduleFolder.GetFiles($"{clip.Name}.psv").FirstOrDefault();
            if (!file.Exists)
                Utils.WriteRedText($"\t\t{clip.Title} does not exist");
            else
            {
                using var fs = DecryptFileHelper.CreateVideo(options.OutputPath, courseTitle, module, clip);
                var stream = new VirtualFileStream(file.FullName);
                stream.Read(fs);

                if (options.CreateTranscript)
                    WriteTranscriptFile(clip, module, courseTitle);
            }
        }

        private void RemoveCourse(Course course)
        {
            Utils.WriteCyanText($"Removing course '{course.Title}' from database.");
            repository.DeleteCourse(course.Name);
            Utils.WriteCyanText($"Deleting course '{course.Title}' folder.");
            var courseFolder = DecryptFileHelper.GetCourseFolder(options.CoursesPath, course.Name);
            courseFolder.Delete(recursive: true);
            Utils.WriteCyanText($"Removing course '{course.Title}' complete");
        }

        public void WriteTranscriptFile(Clip clip, Module module, string courseTitle)
        {
            Utils.WriteBlueText($"\t\t----Writing '{clip.Title}' transcript.");
            var clipTranscripts = repository.GetTranscripts(clip.Id);
            if (clipTranscripts.Count > 0)
            {
                int i = 1;
                using var fs = DecryptFileHelper.CreateVideoTranscript(options.OutputPath, courseTitle, module, clip);
                using var sw = new StreamWriter(fs);
                clipTranscripts.ForEach(clipTranscript =>
                {
                    var start = TimeSpan.FromMilliseconds(clipTranscript.StartTime).ToString(@"hh\:mm\:ss\,fff");
                    var end = TimeSpan.FromMilliseconds(clipTranscript.EndTime).ToString(@"hh\:mm\:ss\,fff");
                    sw.WriteLine(i++);
                    sw.WriteLine(start + " --> " + end);
                    sw.WriteLine(clipTranscript.Text);
                    sw.WriteLine();
                });
            }
        }

    }
}