namespace PluralVideos.Services.Decryptor
{
    public class ClipTranscript
    {
        public int Id { get; set; }

        public int StartTime { get; set; }

        public int EndTime { get; set; }

        public string Text { get; set; }

        public int ClipId { get; set; }

        public virtual Clip Clip { get; set; }
    }
}
