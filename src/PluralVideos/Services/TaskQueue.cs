using PluralVideos.Services.Video;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace PluralVideos.Services
{
    // Thank to https://codereview.stackexchange.com/questions/145938/semaphore-based-concurrent-work-queue
    // https://markheath.net/post/constraining-concurrent-threads-csharp
    public class DownloadEventArgs : EventArgs
    {
        public DownloadEventArgs(bool succeeded, int moduleId, string moduleTitle, int clipId, string clipTitle)
        {
            Succeeded = succeeded;
            ModuleId = moduleId;
            ModuleTitle = moduleTitle;
            ClipId = clipId;
            ClipTitle = clipTitle;
        }

        public bool Succeeded { get; set; }

        public int ModuleId { get; }

        public string ModuleTitle { get; set; }

        public int ClipId { get; set; }

        public string ClipTitle { get; set; }
    }

    public sealed class TaskQueue : IDisposable
    {
        private readonly SemaphoreSlim semaphore;

        private readonly ConcurrentQueue<DownloadClient> clients = new ConcurrentQueue<DownloadClient>();

        public event EventHandler<DownloadEventArgs> ProcessCompleteEvent;

        public TaskQueue()
        {
            var concurrentDownloads = Math.Max(5, Environment.ProcessorCount * 2);
            semaphore = new SemaphoreSlim(concurrentDownloads);
        }

        public void Enqueue(DownloadClient client)
        {
            clients.Enqueue(client);
        }

        public async Task Execute()
        {
            var clips = clients.Count;
            var downloadedClips = 0L;
            var tasks = new List<Task>();
            while (clients.TryDequeue(out var client) || downloadedClips < clips)
            {
                if (client != null)
                {
                    await semaphore.WaitAsync();
                    tasks.Add(Task.Run(async () =>
                    {
                        try
                        {
                            var completed = await client.Download();
                            if (!completed)
                                clients.Enqueue(client);
                            else
                                Interlocked.Increment(ref downloadedClips);

                            OnRaiseDownloadEvent(new DownloadEventArgs(completed, client.ModuleId, client.ModuleTitle, client.Clip.Index, client.Clip.Title));
                        }
                        finally
                        {
                            semaphore.Release();
                        }
                    }));
                }
            }

            await Task.WhenAll(tasks);
        }

        public void Dispose()
        {
            semaphore.Dispose();
        }

        private void OnRaiseDownloadEvent(DownloadEventArgs e)
        {
            ProcessCompleteEvent?.Invoke(this, e);
        }
    }
}
