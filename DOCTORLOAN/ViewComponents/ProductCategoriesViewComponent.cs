using Microsoft.AspNetCore.Mvc;
using DOCTORLOAN.Models.Api;
using DOCTORLOAN.Constants;
using Newtonsoft.Json;

namespace DOCTORLOAN.ViewComponents
{
    public class ProductCategoriesViewComponent : ViewComponent
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public ProductCategoriesViewComponent(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            try
            {
                var httpClient = _httpClientFactory.CreateClient();
                var response = await httpClient.GetAsync(ApiConstants.CategoryFilterCategories);

                if (response.IsSuccessStatusCode)
                {
                    var jsonContent = await response.Content.ReadAsStringAsync();
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

                        ViewBag.BestSellerCategoryId = bestSellerId;
                        return View(productCategories);
                    }
                }
            }
            catch (Exception ex)
            {
                // Log error but don't break the request
                Console.WriteLine($"Error loading categories: {ex.Message}");
            }

            return View(new List<CategoryResponse>());
        }
    }
}

