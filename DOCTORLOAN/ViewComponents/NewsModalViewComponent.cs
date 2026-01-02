using Microsoft.AspNetCore.Mvc;
using DOCTORLOAN.Models.Api;
using DOCTORLOAN.Models.NewsModal;
using DOCTORLOAN.Constants;
using Newtonsoft.Json;

namespace DOCTORLOAN.ViewComponents
{
    public class NewsModalViewComponent : ViewComponent
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly NewsModalConfig _config;

        public NewsModalViewComponent(IHttpClientFactory httpClientFactory, NewsModalConfig config)
        {
            _httpClientFactory = httpClientFactory;
            _config = config;
        }

        public async Task<IViewComponentResult> InvokeAsync(string? keyword = null)
        {
            try
            {
                // Sử dụng keyword từ parameter hoặc config
                var searchKeyword = keyword ?? _config.Keyword;

                var httpClient = _httpClientFactory.CreateClient();
                var response = await httpClient.GetAsync($"{ApiConstants.NewsItemFilterNews}?Keyword={Uri.EscapeDataString(searchKeyword)}");

                if (response.IsSuccessStatusCode)
                {
                    var jsonContent = await response.Content.ReadAsStringAsync();
                    var apiResponse = JsonConvert.DeserializeObject<ApiResponse<NewsListResponse>>(jsonContent);

                    if (apiResponse?.Data?.Items != null && apiResponse.Data.Items.Count > 0)
                    {
                        // Lấy tin tức đầu tiên
                        var newsItem = apiResponse.Data.Items[0];
                        return View(newsItem);
                    }
                }
            }
            catch (Exception ex)
            {
                // Log error but don't break the request
                Console.WriteLine($"Error loading news for modal: {ex.Message}");
            }

            // Return null nếu không có news để ẩn modal
            return View((NewsItemResponse?)null);
        }
    }
}

