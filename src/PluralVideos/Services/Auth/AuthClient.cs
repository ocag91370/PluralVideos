using PluralVideos.Helpers;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace PluralVideos.Services.Auth
{
    public class AuthClient : BaseClient
    {
        private DeviceInfo deviceInfo;

        public AuthClient(Func<bool, Task<string>> getAccessToken, HttpClient httpClientFactory)
            : base(getAccessToken, httpClientFactory)
        {
            var user = DownloadFileHelper.ReadUser();
            deviceInfo = user?.DeviceInfo;
        }

        public async Task<ApiResponse<RegisterResponse>> Autheticate()
        {
            var response = await PostHttp<RegisterResponse>("user/device/unauthenticated", new Register());
            if (response.Success)
                deviceInfo = new DeviceInfo { DeviceId = response.Data.DeviceId, RefreshToken = response.Data.RefreshToken };

            return response;
        }

        public async Task<ApiResponse<User>> GetAccessToken()
        {
            var response = await PostHttp<User>($"user/authorization/{deviceInfo.DeviceId}", deviceInfo);
            if (response.Success)
            {
                var user = response.Data;
                user.DeviceInfo = deviceInfo;
                DownloadFileHelper.WriteUser(user);
            }

            return response;
        }

        public async Task<ApiResponse<DeviceStatus>> DeviceStatus()
        {
            return await GetHttp<DeviceStatus>($"user/device/{deviceInfo.DeviceId}/status");
        }

        public async Task<ApiResponse> Logout()
        {
            if (deviceInfo == null)
            {
                return new ApiResponse
                {
                    Error = new ApiError { Message = "You are not logged in." }
                };
            }

            var response = await DeleteHttp($"user/device/{deviceInfo.DeviceId}");
            if (response.Success)
                DownloadFileHelper.DeleteUser();
            return response;
        }
    }
}
