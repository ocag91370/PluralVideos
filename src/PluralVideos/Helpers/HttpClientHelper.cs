using Newtonsoft.Json;
using System.Net.Http;
using System.Text;

namespace PluralVideos.Helpers
{
    public class HttpClientHelpers
    {
        public static HttpContent GetJsonBody(object value)
        {
            return new StringContent(JsonConvert.SerializeObject(value), Encoding.UTF8, "application/json");
        }
    }
}