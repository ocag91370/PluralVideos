using System;
using System.IO;
using System.Runtime.InteropServices;

namespace PluralVideos.Encryption
{
    public class VirtualFileCache : IDisposable
    {
        private readonly IPsStream encryptedVideoFile;

        public long Length => encryptedVideoFile.Length;

        public VirtualFileCache(string encryptedVideoFilePath)
        {
            encryptedVideoFile = new PsStream(encryptedVideoFilePath);
        }

        public VirtualFileCache(IPsStream stream)
        {
            encryptedVideoFile = stream;
        }

        public void Read(byte[] pv, int offset, int count, IntPtr pcbRead)
        {
            if (Length == 0L)
                return;
            encryptedVideoFile.Seek(offset, SeekOrigin.Begin);
            int length = encryptedVideoFile.Read(pv, 0, count);
            VideoEncryption.DecryptBuffer(pv, length, (long)offset);
            if (!(IntPtr.Zero != pcbRead))
                return;
            Marshal.WriteIntPtr(pcbRead, new IntPtr(length));
        }

        public void Dispose()
        {
            encryptedVideoFile.Dispose();
        }
    }
}
