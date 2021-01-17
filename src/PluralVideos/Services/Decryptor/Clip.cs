using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace PluralVideos.Services.Decryptor
{
    public partial class Clip
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Title { get; set; }

        public int ClipIndex { get; set; }

        public int DurationInMilliseconds { get; set; }

        public int SupportsStandard { get; set; }

        public int SupportsWidescreen { get; set; }

        public int ModuleId { get; set; }

        public virtual Module Module { get; set; }

        public virtual ICollection<ClipTranscript> ClipTranscript { get; set; }

        public Clip()
        {
            ClipTranscript = new Collection<ClipTranscript>();
        }
    }
}
