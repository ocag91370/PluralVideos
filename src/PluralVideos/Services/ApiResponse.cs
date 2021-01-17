using Newtonsoft.Json;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace PluralVideos.Services
{
    public class ApiResponse
    {
        public bool Success => Error == null;

        public HttpResponseMessage Message { get; set; }

        public string ResponseBody { get; set; }

        public ApiError Error { get; set; }


        public static async Task<ApiResponse> FromMessage(HttpResponseMessage message)
        {
            var response = new ApiResponse
            {
                Message = message,
                ResponseBody = await message.Content.ReadAsStringAsync()
            };

            if (!message.IsSuccessStatusCode)
                response.HandleFailedCall();

            return response;
        }

        protected void HandleFailedCall()
        {
            try
            {
                Error = JsonConvert.DeserializeObject<ApiError>(ResponseBody) ?? new ApiError();

                if (Error.Message == null)
                {
                    Error = new ApiError()
                    {
                        Message = !string.IsNullOrEmpty(ResponseBody) ? ResponseBody : Message.StatusCode.ToString()
                    };
                }
            }
            catch
            {
                Error = new ApiError()
                {
                    Message = !string.IsNullOrEmpty(ResponseBody) ? ResponseBody : Message.StatusCode.ToString()
                };
            }
        }
    }

    public class ApiResponse<T> : ApiResponse
    {
        public T Data { get; set; }

        public new static async Task<ApiResponse<T>> FromMessage(HttpResponseMessage message)
        {
            var response = new ApiResponse<T>
            {
                Message = message,
                ResponseBody = await message.Content.ReadAsStringAsync()
            };

            if (message.IsSuccessStatusCode)
                response.Data = JsonConvert.DeserializeObject<T>(response.ResponseBody);
            else
                response.HandleFailedCall();

            return response;
        }
    }

    public class ApiFile : ApiResponse<Stream>
    {
        public new static async Task<ApiFile> FromMessage(HttpResponseMessage message)
        {
            var response = new ApiFile
            {
                Message = message,
            };

            if (message.IsSuccessStatusCode)
                response.Data = await message.Content.ReadAsStreamAsync();
            else
                response.HandleFailedCall();

            return response;
        }
    }

    public class ApiError
    {
        public string Code { get; set; }
        public string Message { get; set; }
        public string PropertyName { get; set; }
    }
}
