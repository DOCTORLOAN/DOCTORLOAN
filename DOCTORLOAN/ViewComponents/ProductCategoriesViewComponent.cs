// <copyright file="ProductCategoriesViewComponent.cs" company="DOCTORLOAN">
// Copyright (c) DOCTORLOAN. All rights reserved.
// </copyright>

using DOCTORLOAN.Constants;
using DOCTORLOAN.Models.Api;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace DOCTORLOAN.ViewComponents
{
    public class ProductCategoriesViewComponent : ViewComponent
    {
        private readonly IHttpClientFactory httpClientFactory;
        private readonly ILogger<ProductCategoriesViewComponent> logger;

        public ProductCategoriesViewComponent(IHttpClientFactory httpClientFactory, ILogger<ProductCategoriesViewComponent> logger)
        {
            this.httpClientFactory = httpClientFactory;
            this.logger = logger;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            try
            {
                using var httpClient = this.httpClientFactory.CreateClient();
                var uri = new Uri(ApiConstants.CategoryFilterCategories);
                var response = await httpClient.GetAsync(uri).ConfigureAwait(false);

                if (response.IsSuccessStatusCode)
                {
                    var jsonContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    var apiResponse = JsonConvert.DeserializeObject<ApiResponse<CategoryListResponse>>(jsonContent);

                    if (apiResponse?.Data?.Items != null)
                    {
                        var categories = apiResponse.Data.Items;

                        // Tìm category "bestsaller-doctorloan"
                        var bestSellerCategory = categories.FirstOrDefault(c => c.Slug == "bestsaller-doctorloan");
                        var bestSellerId = bestSellerCategory?.Id ?? 0;

                        // Lọc các categories con của bestseller
                        var productCategories = categories
                            .Where(c => c.ParentId == bestSellerId)
                            .ToList();

                        this.ViewBag.BestSellerCategoryId = bestSellerId;
                        return this.View(productCategories);
                    }
                }
            }
            catch (HttpRequestException ex)
            {
                this.logger.LogError(ex, "HTTP error occurred while loading categories");
            }
            catch (TaskCanceledException ex)
            {
                this.logger.LogError(ex, "Request timeout occurred while loading categories");
            }
            catch (JsonException ex)
            {
                this.logger.LogError(ex, "JSON deserialization error occurred while loading categories");
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "Unexpected error occurred while loading categories");
            }

            return this.View(new List<CategoryResponse>());
        }
    }
}
