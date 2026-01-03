// <copyright file="ProductsController.cs" company="DOCTORLOAN">
// Copyright (c) DOCTORLOAN. All rights reserved.
// </copyright>

using System.Text;
using DOCTORLOAN.Constants;
using DOCTORLOAN.Helpers;
using DOCTORLOAN.Models.Api;
using DOCTORLOAN.Models.Products;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace DOCTORLOAN.Controllers
{
    [AllowAnonymous]
    public class ProductsController : Controller
    {
        private readonly IHttpClientFactory httpClientFactory;
        private readonly ILogger<ProductsController> logger;

        public ProductsController(IHttpClientFactory httpClientFactory, ILogger<ProductsController> logger)
        {
            this.httpClientFactory = httpClientFactory;
            this.logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            try
            {
                // Set loading state
                this.SetLoadingState(true, "Đang tải danh sách sản phẩm...");
                this.SetErrorState(false);

                using var httpClient = this.httpClientFactory.CreateClient();
                var uri = new Uri(ApiConstants.ProductFilterProducts);
                var response = await httpClient.GetAsync(uri).ConfigureAwait(false);

                if (response.IsSuccessStatusCode)
                {
                    var jsonContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
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
                                        Value = po.Value,
                                    }).ToList() ?? new List<ProductOptionViewModel>(),
                                }).ToList() ?? new List<ProductItemViewModel>(),
                            }).ToList(),
                        };

                        // Set loading state to false after data loaded
                        this.SetLoadingState(false);
                        return this.View(viewModel);
                    }
                }

                // Set error state if no data
                this.SetLoadingState(false);
                this.SetErrorState(true, "Không thể tải danh sách sản phẩm. Vui lòng thử lại sau.");
                return this.View(new ProductListViewModel());
            }
            catch (HttpRequestException ex)
            {
                this.logger.LogError(ex, "HTTP error occurred while loading products");
                this.SetLoadingState(false);
                this.SetErrorState(true, "Không thể kết nối đến server. Vui lòng thử lại sau.");
                return this.View(new ProductListViewModel());
            }
            catch (TaskCanceledException ex)
            {
                this.logger.LogError(ex, "Request timeout occurred while loading products");
                this.SetLoadingState(false);
                this.SetErrorState(true, "Request timeout. Vui lòng thử lại sau.");
                return this.View(new ProductListViewModel());
            }
            catch (JsonException ex)
            {
                this.logger.LogError(ex, "JSON deserialization error occurred while loading products");
                this.SetLoadingState(false);
                this.SetErrorState(true, "Lỗi xử lý dữ liệu. Vui lòng thử lại sau.");
                return this.View(new ProductListViewModel());
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "Unexpected error occurred while loading products");
                // Set error state
                this.SetLoadingState(false);
                this.SetErrorState(true, $"Đã xảy ra lỗi: {ex.Message}");
                return this.View(new ProductListViewModel());
            }
        }

        [HttpGet]
        public async Task<IActionResult> ProductDetail(int? productId, int? categoryId)
        {
            try
            {
                if (productId == null && categoryId == null)
                {
                    return this.RedirectToAction("Index", "Home");
                }

                // Set loading state
                this.SetLoadingState(true, "Đang tải thông tin sản phẩm...");
                this.SetErrorState(false);

                using var httpClient = this.httpClientFactory.CreateClient();
                int? actualProductId = productId;

                // Nếu có categoryId, lấy sản phẩm đầu tiên trong category
                if (categoryId != null && productId == null)
                {
                    var categoryUri = new Uri($"{ApiConstants.ProductFilterProducts}?CategoryId={categoryId}");
                    var categoryResponse = await httpClient.GetAsync(categoryUri).ConfigureAwait(false);
                    if (categoryResponse.IsSuccessStatusCode)
                    {
                        var categoryJson = await categoryResponse.Content.ReadAsStringAsync().ConfigureAwait(false);
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
                    return this.RedirectToAction("Index", "Home");
                }

                // Lấy chi tiết sản phẩm
                var productUri = new Uri($"{ApiConstants.ProductGetProduct}?id={actualProductId}");
                var productResponse = await httpClient.GetAsync(productUri).ConfigureAwait(false);
                if (productResponse.IsSuccessStatusCode)
                {
                    var productJson = await productResponse.Content.ReadAsStringAsync().ConfigureAwait(false);
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
                                OrderBy = pm.OrderBy,
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
                                    Value = po.Value,
                                }).ToList() ?? new List<ProductOptionViewModel>(),
                            }).ToList() ?? new List<ProductItemViewModel>(),
                            ProductAttributes = product.ProductAttributes?.Select(pa => new ProductAttributeViewModel
                            {
                                ProductId = pa.ProductId,
                                AttributeId = pa.AttributeId,
                                Value = pa.Value,
                            }).ToList() ?? new List<ProductAttributeViewModel>(),
                            ProductDetails = product.ProductDetails?.Select(pd => new ProductDetailInfoViewModel
                            {
                                ProductId = pd.ProductId,
                                Description = pd.Description,
                                Summary = pd.Summary,
                            }).ToList() ?? new List<ProductDetailInfoViewModel>(),
                        };

                        // Lấy sản phẩm liên quan (Best Seller)
                        var categoryListUri = new Uri(ApiConstants.CategoryFilterCategories);
                        var categoryListResponse = await httpClient.GetAsync(categoryListUri).ConfigureAwait(false);
                        if (categoryListResponse.IsSuccessStatusCode)
                        {
                            var categoryJson = await categoryListResponse.Content.ReadAsStringAsync().ConfigureAwait(false);
                            var categoryApiResponse = JsonConvert.DeserializeObject<ApiResponse<CategoryListResponse>>(categoryJson);

                            if (categoryApiResponse?.Data?.Items != null)
                            {
                                var bestSellerCategory = categoryApiResponse.Data.Items.FirstOrDefault(c => c.Slug == "bestsaller-doctorloan");
                                if (bestSellerCategory != null)
                                {
                                    var bestSellerUri = new Uri($"{ApiConstants.ProductFilterProducts}?CategoryId={bestSellerCategory.Id}");
                                    var bestSellerResponse = await httpClient.GetAsync(bestSellerUri).ConfigureAwait(false);
                                    if (bestSellerResponse.IsSuccessStatusCode)
                                    {
                                        var bestSellerJson = await bestSellerResponse.Content.ReadAsStringAsync().ConfigureAwait(false);
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
                                                    Price = pi.Price,
                                                }).ToList() ?? new List<ProductItemViewModel>(),
                                            }).ToList();
                                        }
                                    }
                                }
                            }
                        }

                        // Set loading state to false after data loaded
                        this.SetLoadingState(false);
                        return this.View(viewModel);
                    }
                }

                // Set error state
                this.SetLoadingState(false);
                this.SetErrorState(true, "Không thể tải thông tin sản phẩm. Vui lòng thử lại sau.");
                return this.RedirectToAction("Index", "Home");
            }
            catch (HttpRequestException ex)
            {
                this.logger.LogError(ex, "HTTP error occurred while loading product detail");
                this.SetLoadingState(false);
                this.SetErrorState(true, "Không thể kết nối đến server. Vui lòng thử lại sau.");
                return this.RedirectToAction("Index", "Home");
            }
            catch (TaskCanceledException ex)
            {
                this.logger.LogError(ex, "Request timeout occurred while loading product detail");
                this.SetLoadingState(false);
                this.SetErrorState(true, "Request timeout. Vui lòng thử lại sau.");
                return this.RedirectToAction("Index", "Home");
            }
            catch (JsonException ex)
            {
                this.logger.LogError(ex, "JSON deserialization error occurred while loading product detail");
                this.SetLoadingState(false);
                this.SetErrorState(true, "Lỗi xử lý dữ liệu. Vui lòng thử lại sau.");
                return this.RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "Unexpected error occurred while loading product detail");
                // Set error state
                this.SetLoadingState(false);
                this.SetErrorState(true, $"Đã xảy ra lỗi: {ex.Message}");
                return this.RedirectToAction("Index", "Home");
            }
        }
    }
}
