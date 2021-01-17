using PluralVideos.Options;
using System;
using System.Threading;
using System.Threading.Tasks;
using PluralVideos.Helpers;
using PluralVideos.Services;

namespace PluralVideos
{
    public class Authenticator
    {
        private readonly AuthenticatorOptions options;

        private readonly PluralSightApi api = new PluralSightApi(15);

        public Authenticator(AuthenticatorOptions options)
        {
            this.options = options;
        }

        public async Task Run()
        {
            if (options.Login && options.Logout)
                throw new Exception("Cannot use both --login and --logout flags");

            if (options.Login)
                await Login();

            if (options.Logout)
            {
                var response = await api.Auth.Logout();
                if (response.Success)
                    Utils.WriteGreenText("Logged out successfully");
                else
                    Utils.WriteRedText($"Could not log out . Error: {response.Error.Message}");
            }
        }

        public async Task Login()
        {
            var user = DownloadFileHelper.ReadUser();
            if (user != null)
            {
                Utils.WriteGreenText("You are already logged in.");
                return;
            }

            var authResponse = await api.Auth.Autheticate();
            if (!authResponse.Success)
            {
                Utils.WriteRedText($"Could not register the device. Error: {authResponse.Error.Message}");
                return;
            }

            var register = authResponse.Data;
            Utils.WriteYellowText($"Visit {register.AuthDeviceUrl}");
            Utils.WriteYellowText($"Enter Pin {register.Pin}");
            Utils.WriteYellowText($"Expires at: {register.ValidUntil:HH:mm} UTC");

            while (true)
            {
                Thread.Sleep(15000);

                if (register.ValidUntil <= DateTimeOffset.UtcNow)
                {
                    Utils.WriteRedText("Pin is no longer valid.");
                    break;
                }

                var statusResponse = await api.Auth.DeviceStatus();
                if (statusResponse.Success && statusResponse.Data.Status == "Valid")
                {
                    var authorizeResponse = await api.Auth.GetAccessToken();
                    if (!authorizeResponse.Success)
                        Utils.WriteRedText($"Could not get access token. Error: {authorizeResponse.Error.Message}");
                    else
                        Utils.WriteGreenText("You have successfully logged in");

                    break;
                }
                else if (!statusResponse.Success)
                {
                    Utils.WriteRedText($"Could not get device status. Error: {statusResponse.Error.Message}");
                }
            }
        }
    }
}
