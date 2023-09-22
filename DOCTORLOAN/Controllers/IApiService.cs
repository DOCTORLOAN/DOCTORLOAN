using DOCTORLOAN.Models.Users;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;

namespace DOCTORLOAN.Controllers
{
    public interface IApiService
    {
        Task<T> GetAsync<T>(string endpoint);
        Task<TResponse> PostAsync<TRequest, TResponse>(string endpoint, TRequest request);
        // Các phương thức khác cần thiết
    }

    public class ApiService : IApiService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public ApiService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<T> GetAsync<T>(string endpoint)
        {
            var client = _httpClientFactory.CreateClient("MyApi");
            var response = await client.GetAsync(endpoint);

            // Xử lý và phân tích kết quả
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<T>(content);
            }
            else
            {
                // Xử lý lỗi ở đây
                throw new Exception("Failed to call the API.");
            }
        }

        public async Task<TResponse> PostAsync<TRequest, TResponse>(string endpoint, TRequest request)
        {
            var client = _httpClientFactory.CreateClient("MyApi");
            var requestContent = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
            var response = await client.PostAsync(endpoint, requestContent);

            // Xử lý và phân tích kết quả
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<TResponse>(content);
            }
            else
            {
                // Xử lý lỗi ở đây
                throw new Exception("Failed to call the API.");
            }
        }
    }
}
