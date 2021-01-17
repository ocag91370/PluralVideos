using PluralVideos.Services.Video;
using System.Collections.Generic;
using System.Linq;

namespace PluralVideos.Extensions
{
    public static class ICollectionsExtensions
    {
        public static IEnumerable<(T item, int index)> WithIndex<T>(this IEnumerable<T> source)
        {
            return source.Select((item, index) => (item, index));
        }

        public static (Clip, int, string) GetClip(this Course course, string clipId)
        {
            var items = course.Modules.Select((m, index) => (m.Clips.FirstOrDefault(c => c.Id == clipId), index, m.Title));
            return items.FirstOrDefault(i => i.Item1 != null);
        }
    }
}
