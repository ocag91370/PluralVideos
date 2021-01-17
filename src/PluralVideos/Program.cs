using CommandLine;
using PluralVideos.Helpers;
using PluralVideos.Options;
using System;
using System.Threading.Tasks;

namespace PluralVideos
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var result = Parser.Default.ParseArguments<AuthenticatorOptions, DecryptorOptions, DownloaderOptions>(args);
            await result.WithParsedAsync<AuthenticatorOptions>(RunAuthOptions);
            await result.WithParsedAsync<DownloaderOptions>(RunDownloadOptions);
            result.WithParsed<DecryptorOptions>(RunDecryptorOptions);
        }

        private static async Task RunAuthOptions(AuthenticatorOptions options)
        {
            try
            {
                var authenticator = new Authenticator(options);
                await authenticator.Run();
            }
            catch (Exception exception)
            {
                Utils.WriteRedText($"Error occured: {exception.Message}");
            }
        }

        private static async Task RunDownloadOptions(DownloaderOptions options)
        {
            try
            {
                var downloader = new Downloader(options);
                await downloader.Download();
            }
            catch (Exception exception)
            {
                Utils.WriteRedText($"Error occured: {exception.Message}");
            }
        }

        private static void RunDecryptorOptions(DecryptorOptions options)
        {
            try
            {
                var decryptor = new Decryptor(options);
                decryptor.DecryptCourses();
            }
            catch (Exception exception)
            {
                Utils.WriteRedText($"Error occured: {exception.Message}");
            }
        }
    }
}
