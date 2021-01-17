using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;

namespace PluralVideos.Encryption
{
    public class VirtualFileStream : IStream, IDisposable
    {
        private readonly object _Lock = new object();
        private long position;
        private readonly VirtualFileCache _Cache;

        public VirtualFileStream(string EncryptedVideoFilePath)
        {
            _Cache = new VirtualFileCache(EncryptedVideoFilePath);
        }

        private VirtualFileStream(VirtualFileCache Cache)
        {
            _Cache = Cache;
        }

        public void Read(byte[] pv, int cb, IntPtr pcbRead)
        {
            if (position < 0L || position > _Cache.Length)
            {
                Marshal.WriteIntPtr(pcbRead, new IntPtr(0));
            }
            else
            {
                lock (_Lock)
                {
                    _Cache.Read(pv, (int)position, cb, pcbRead);
                    position += pcbRead.ToInt64();
                }
            }
        }

        public void Read(FileStream stream)
        {
            var size = (int)_Cache.Length;
            var buffer = new byte[size];
            Read(buffer, size, (IntPtr)0);
            stream.Write(buffer, 0, size);
        }

        public void Write(byte[] pv, int cb, IntPtr pcbWritten)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void Clone(out IStream ppstm)
        {
            ppstm = new VirtualFileStream(_Cache);
        }

        public void Seek(long dlibMove, int dwOrigin, IntPtr plibNewPosition)
        {
            var seekOrigin = (SeekOrigin)dwOrigin;
            lock (_Lock)
            {
                switch (seekOrigin)
                {
                    case SeekOrigin.Begin:
                        position = dlibMove;
                        break;
                    case SeekOrigin.Current:
                        position += dlibMove;
                        break;
                    case SeekOrigin.End:
                        position = _Cache.Length + dlibMove;
                        break;
                }
                if (!(IntPtr.Zero != plibNewPosition))
                    return;
                Marshal.WriteInt64(plibNewPosition, position);
            }
        }

        public void Stat(out STATSTG pstatstg, int grfStatFlag)
        {
            pstatstg = new STATSTG
            {
                cbSize = _Cache.Length
            };
        }

        public void Commit(int grfCommitFlags)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void CopyTo(IStream pstm, long cb, IntPtr pcbRead, IntPtr pcbWritten)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void LockRegion(long libOffset, long cb, int dwLockType)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void Revert()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void SetSize(long libNewSize)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void UnlockRegion(long libOffset, long cb, int dwLockType)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void Dispose()
        {
            _Cache.Dispose();
        }
    }
}
