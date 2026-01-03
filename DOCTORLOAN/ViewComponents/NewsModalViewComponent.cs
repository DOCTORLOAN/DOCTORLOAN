// <copyright file="NewsModalViewComponent.cs" company="DOCTORLOAN">
// Copyright (c) DOCTORLOAN. All rights reserved.
// </copyright>

using DOCTORLOAN.Constants;
using DOCTORLOAN.Models.Api;
using DOCTORLOAN.Models.NewsModal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace DOCTORLOAN.ViewComponents
{
    public class NewsModalViewComponent : ViewComponent
    {
        private readonly IHttpClientFactory httpClientFactory;
        private readonly NewsModalConfig config;
        private readonly ILogger<NewsModalViewComponent> logger;

        public NewsModalViewComponent(
            IHttpClientFactory httpClientFactory,
            NewsModalConfig config,
            ILogger<NewsModalViewComponent> logger)
        {
            this.httpClientFactory = httpClientFactory;
            this.config = config;
            this.logger = logger;
        }

        public async Task<IViewComponentResult> InvokeAsync(string? keyword = null)
        {
            try
            {
                // Sử dụng keyword từ parameter hoặc config
                var searchKeyword = keyword ?? this.config.Keyword;

                using var httpClient = this.httpClientFactory.CreateClient();
                var uri = new Uri($"{ApiConstants.NewsItemFilterNews}?Keyword={Uri.EscapeDataString(searchKeyword)}");
                var response = await httpClient.GetAsync(uri).ConfigureAwait(false);

                if (response.IsSuccessStatusCode)
                {
                    var jsonContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    var apiResponse = JsonConvert.DeserializeObject<ApiResponse<NewsListResponse>>(jsonContent);

                    if (apiResponse?.Data?.Items != null && apiResponse.Data.Items.Count > 0)
                    {
                        // Lấy tin tức đầu tiên
                        var newsItem = apiResponse.Data.Items[0];
                        return this.View(newsItem);
                    }
                }
            }
            catch (HttpRequestException ex)
            {
                this.logger.LogError(ex, "HTTP error occurred while loading news for modal");
            }
            catch (TaskCanceledException ex)
            {
                this.logger.LogError(ex, "Request timeout occurred while loading news for modal");
            }
            catch (JsonException ex)
            {
                this.logger.LogError(ex, "JSON deserialization error occurred while loading news for modal");
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "Unexpected error occurred while loading news for modal");
            }

            // Return null nếu không có news để ẩn modal
            return this.View((NewsItemResponse?)null);
        }
    }
}
