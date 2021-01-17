using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace PluralVideos.Services.Decryptor
{
    public partial class Module
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Title { get; set; }

        public string AuthorHandle { get; set; }

        public string Description { get; set; }

        public int DurationInMilliseconds { get; set; }

        public int ModuleIndex { get; set; }

        public string CourseName { get; set; }

        public virtual Course Course { get; set; }

        public virtual ICollection<Clip> Clip { get; set; }

        public Module()
        {
            Clip = new Collection<Clip>();
        }
    }
}
