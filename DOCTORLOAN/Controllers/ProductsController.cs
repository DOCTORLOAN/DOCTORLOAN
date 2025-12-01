using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using DOCTORLOAN.Models.Products;
using DOCTORLOAN.Models.Api;
using DOCTORLOAN.Helpers;
using DOCTORLOAN.Constants;
using Newtonsoft.Json;
using System.Text;

namespace DOCTORLOAN.Controllers
{
    [AllowAnonymous]
    public class ProductsController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public ProductsController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                // Set loading state
                this.SetLoadingState(true, "Đang tải danh sách sản phẩm...");
                this.SetErrorState(false);

                var httpClient = _httpClientFactory.CreateClient();
                var response = await httpClient.GetAsync(ApiConstants.ProductFilterProducts);

                if (response.IsSuccessStatusCode)
                {
                    var jsonContent = await response.Content.ReadAsStringAsync();
                    var apiResponse = JsonConvert.DeserializeObject<ApiResponse<ProductListResponse>>(jsonContent);

                    if (apiResponse?.Data?.Items != null)
                    {
                        var viewModel = new ProductListViewModel
                        {
                            Products = apiResponse.Data.Items.Select(item => new ProductViewModel
                            {
                                Id = item.Id,
                                Name = item.Name,
                                Sku = item.Sku,
                                ImageUrl = item.ImageUrl,
                                Summary = item.Summary,
                                Price = item.Price,
                                ProductItems = item.ProductItems?.Select(pi => new ProductItemViewModel
                                {
                                    Id = pi.Id,
                                    Name = pi.Name,
                                    Sku = pi.Sku,
                                    Price = pi.Price,
                                    ProductOptions = pi.ProductOptions?.Select(po => new ProductOptionViewModel
                                    {
                                        Id = po.Id,
                                        Name = po.Name,
                                        Value = po.Value
                                    }).ToList() ?? new List<ProductOptionViewModel>()
                                }).ToList() ?? new List<ProductItemViewModel>()
                            }).ToList()
                        };

                        // Set loading state to false after data loaded
                        this.SetLoadingState(false);
                        return View(viewModel);
                    }
                }

                // Set error state if no data
                this.SetLoadingState(false);
                this.SetErrorState(true, "Không thể tải danh sách sản phẩm. Vui lòng thử lại sau.");
                return View(new ProductListViewModel());
            }
            catch (Exception ex)
            {
                // Set error state
                this.SetLoadingState(false);
                this.SetErrorState(true, $"Đã xảy ra lỗi: {ex.Message}");
                return View(new ProductListViewModel());
            }
        }

        public async Task<IActionResult> ProductDetail(int? productId, int? categoryId)
        {
            try
            {
                if (productId == null && categoryId == null)
                {
                    return RedirectToAction("Index", "Home");
                }

                // Set loading state
                this.SetLoadingState(true, "Đang tải thông tin sản phẩm...");
                this.SetErrorState(false);

                var httpClient = _httpClientFactory.CreateClient();
                int? actualProductId = productId;

                // Nếu có categoryId, lấy sản phẩm đầu tiên trong category
                if (categoryId != null && productId == null)
                {
                    var categoryResponse = await httpClient.GetAsync($"{ApiConstants.ProductFilterProducts}?CategoryId={categoryId}");
                    if (categoryResponse.IsSuccessStatusCode)
                    {
                        var categoryJson = await categoryResponse.Content.ReadAsStringAsync();
                        var categoryApiResponse = JsonConvert.DeserializeObject<ApiResponse<ProductListResponse>>(categoryJson);
                        if (categoryApiResponse?.Data?.Items != null && categoryApiResponse.Data.Items.Count > 0)
                        {
                            actualProductId = categoryApiResponse.Data.Items[0].Id;
                        }
                    }
                }

                if (actualProductId == null)
                {
                    this.SetLoadingState(false);
                    this.SetErrorState(true, "Không tìm thấy sản phẩm.");
                    return RedirectToAction("Index", "Home");
                }

                // Lấy chi tiết sản phẩm
                var productResponse = await httpClient.GetAsync($"{ApiConstants.ProductGetProduct}?id={actualProductId}");
                if (productResponse.IsSuccessStatusCode)
                {
                    var productJson = await productResponse.Content.ReadAsStringAsync();
                    var productApiResponse = JsonConvert.DeserializeObject<ApiResponse<ProductDetailResponse>>(productJson);

                    if (productApiResponse?.Data != null)
                    {
                        var product = productApiResponse.Data;
                        var viewModel = new ProductDetailViewModel
                        {
                            Id = product.Id,
                            Name = product.Name,
                            Sku = product.Sku,
                            Price = product.Price,
                            ImageUrl = product.ImageUrl,
                            ProductMedias = product.ProductMedias?.Select(pm => new ProductMediaViewModel
                            {
                                MediaId = pm.MediaId,
                                ProductId = pm.ProductId,
                                MediaUrl = pm.MediaUrl,
                                ItemCode = pm.ItemCode,
                                OrderBy = pm.OrderBy
                            }).ToList() ?? new List<ProductMediaViewModel>(),
                            ProductItems = product.ProductItems?.Select(pi => new ProductItemViewModel
                            {
                                Id = pi.Id,
                                Name = pi.Name,
                                Sku = pi.Sku,
                                Price = pi.Price,
                                ProductOptions = pi.ProductOptions?.Select(po => new ProductOptionViewModel
                                {
                                    Id = po.Id,
                                    Name = po.Name,
                                    Value = po.Value
                                }).ToList() ?? new List<ProductOptionViewModel>()
                            }).ToList() ?? new List<ProductItemViewModel>(),
                            ProductAttributes = product.ProductAttributes?.Select(pa => new ProductAttributeViewModel
                            {
                                ProductId = pa.ProductId,
                                AttributeId = pa.AttributeId,
                                Value = pa.Value
                            }).ToList() ?? new List<ProductAttributeViewModel>(),
                            ProductDetails = product.ProductDetails?.Select(pd => new ProductDetailInfoViewModel
                            {
                                ProductId = pd.ProductId,
                                Description = pd.Description,
                                Summary = pd.Summary
                            }).ToList() ?? new List<ProductDetailInfoViewModel>()
                        };

                        // Lấy sản phẩm liên quan (Best Seller)
                        var categoryResponse = await httpClient.GetAsync(ApiConstants.CategoryFilterCategories);
                        if (categoryResponse.IsSuccessStatusCode)
                        {
                            var categoryJson = await categoryResponse.Content.ReadAsStringAsync();
                            var categoryApiResponse = JsonConvert.DeserializeObject<ApiResponse<CategoryListResponse>>(categoryJson);
                            
                            if (categoryApiResponse?.Data?.Items != null)
                            {
                                var bestSellerCategory = categoryApiResponse.Data.Items.FirstOrDefault(c => c.Slug == "bestsaller-doctorloan");
                                if (bestSellerCategory != null)
                                {
                                    var bestSellerResponse = await httpClient.GetAsync($"{ApiConstants.ProductFilterProducts}?CategoryId={bestSellerCategory.Id}");
                                    if (bestSellerResponse.IsSuccessStatusCode)
                                    {
                                        var bestSellerJson = await bestSellerResponse.Content.ReadAsStringAsync();
                                        var bestSellerApiResponse = JsonConvert.DeserializeObject<ApiResponse<ProductListResponse>>(bestSellerJson);
                                        
                                        if (bestSellerApiResponse?.Data?.Items != null)
                                        {
                                            viewModel.RelatedProducts = bestSellerApiResponse.Data.Items.Select(item => new ProductViewModel
                                            {
                                                Id = item.Id,
                                                Name = item.Name,
                                                Sku = item.Sku,
                                                ImageUrl = item.ImageUrl,
                                                Summary = item.Summary,
                                                Price = item.Price,
                                                ProductItems = item.ProductItems?.Select(pi => new ProductItemViewModel
                                                {
                                                    Id = pi.Id,
                                                    Name = pi.Name,
                                                    Sku = pi.Sku,
                                                    Price = pi.Price
                                                }).ToList() ?? new List<ProductItemViewModel>()
                                            }).ToList();
                                        }
                                    }
                                }
                            }
                        }

                        // Set loading state to false after data loaded
                        this.SetLoadingState(false);
                        return View(viewModel);
                    }
                }

                // Set error state
                this.SetLoadingState(false);
                this.SetErrorState(true, "Không thể tải thông tin sản phẩm. Vui lòng thử lại sau.");
                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                // Set error state
                this.SetLoadingState(false);
                this.SetErrorState(true, $"Đã xảy ra lỗi: {ex.Message}");
                return RedirectToAction("Index", "Home");
            }
        }
    }
}
