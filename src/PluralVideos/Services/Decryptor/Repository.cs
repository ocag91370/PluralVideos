using PluralVideos.Options;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PluralVideos.Services.Decryptor
{
    public class Repository : IDisposable
    {
        private readonly PluralSightContext context;

        public Repository(DecryptorOptions options)
        {
            context = new PluralSightContext(options);
        }

        /// <summary>
        /// Get transcript text of specified clip from database.
        /// </summary>
        /// <param name="clipId">Clip Id</param>
        /// <returns>List of transcript text of the current clip.</returns>
        public List<ClipTranscript> GetTranscripts(int clipId)
        {
            return context.ClipTranscript
                .Where(t => t.ClipId == clipId)
                .ToList();
        }

        /// <summary>
        /// Get all modules information of specified course from database.
        /// </summary>
        /// <param name="courseName">Name of course</param>
        /// <returns>List of modules information of specified course.</returns>
        public List<Module> GetModules(string courseName)
        {
            return context.Module
                .Where(m => m.CourseName == courseName)
                .ToList();
        }

        /// <summary>
        /// Get all clips information of specified module from database.
        /// </summary>
        /// <param name="moduleId">Module Id</param>
        /// <returns>List of information about clips belong to specifed module.</returns>
        public List<Clip> GetClips(int moduleId)
        {
            return context.Clip
                .Where(c => c.ModuleId == moduleId)
                .ToList();
        }

        public List<Course> GetCourses()
        {
            return context.Course
                .ToList(); ;
        }

        /// <summary>
        /// Get course information from database.
        /// </summary>
        /// <param name="folderCoursePath">Folder contains all courses</param>
        /// <returns>Course information</returns>
        public Course GetCourse(string courseName)
        {
            return context.Course
                .FirstOrDefault(c => c.Name == courseName);
        }

        public bool DeleteCourse(string courseName)
        {
            var course = GetCourse(courseName);
            if (course != null)
            {
                context.Course.Remove(course);
                context.SaveChanges();
                return true;
            }
            return false;
        }

        public void Dispose()
        {
            context.Dispose();
        }
    }
}
