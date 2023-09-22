using Newtonsoft.Json;
using System.Text;

namespace DOCTORLOAN.service
{
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

            // Check the result res
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<T>(content);
            }
            else
            {
                throw new Exception("Failed to call the API.");
            }
        }

        public async Task<TResponse> PostAsync<TRequest, TResponse>(string endpoint, TRequest request)
        {
            var client = _httpClientFactory.CreateClient("MyApi");
            var requestContent = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
            var response = await client.PostAsync(endpoint, requestContent);

            // Check the result res
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<TResponse>(content);
            }
            else
            {
                throw new Exception("Failed to call the API.");
            }
        }
    }
}