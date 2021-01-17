using System.IO;

namespace PluralVideos.Encryption
{
    public class PsStream : IPsStream
    {
        private readonly Stream fileStream;

        public long Length { get; private set; }

        public int BlockSize => 262144;

        public PsStream(string filenamePath)
        {
            fileStream = File.Open(filenamePath, FileMode.Open, FileAccess.Read, FileShare.Read);
            Length = new FileInfo(filenamePath).Length;
        }

        public void Seek(int offset, SeekOrigin begin)
        {
            if (Length <= 0L)
                return;
            fileStream.Seek(offset, begin);
        }

        public int Read(byte[] pv, int i, int count) =>
            Length <= 0L ? 0 : fileStream.Read(pv, i, count);

        public void Dispose()
        {
            Length = 0L;
            fileStream.Dispose();
        }
    }
}
