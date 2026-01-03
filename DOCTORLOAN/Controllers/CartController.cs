// <copyright file="CartController.cs" company="DOCTORLOAN">
// Copyright (c) DOCTORLOAN. All rights reserved.
// </copyright>

using System.Text;
using DOCTORLOAN.Constants;
using DOCTORLOAN.Helpers;
using DOCTORLOAN.Models.Api;
using DOCTORLOAN.Models.Orders;
using DOCTORLOAN.Models.Products;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace DOCTORLOAN.Controllers
{
    [AllowAnonymous]
    public class CartController : Controller
    {
        private readonly IHttpClientFactory httpClientFactory;
        private readonly ILogger<CartController> logger;

        public CartController(IHttpClientFactory httpClientFactory, ILogger<CartController> logger)
        {
            this.httpClientFactory = httpClientFactory;
            this.logger = logger;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return this.View();
        }

        [HttpGet]
        public IActionResult AddToCart()
        {
            return this.View();
        }

        [HttpGet]
        public async Task<IActionResult> Payment(int id, int quantity)
        {
            try
            {
                if (id <= 0 || quantity <= 0)
                {
                    return this.RedirectToAction("Index", "Home");
                }

                // Set loading state
                this.SetLoadingState(true, "Đang tải thông tin sản phẩm...");
                this.SetErrorState(false);

                using var httpClient = this.httpClientFactory.CreateClient();
                var productUri = new Uri($"{ApiConstants.ProductGetProduct}?id={id}");
                var productResponse = await httpClient.GetAsync(productUri).ConfigureAwait(false);

                if (productResponse.IsSuccessStatusCode)
                {
                    var productJson = await productResponse.Content.ReadAsStringAsync().ConfigureAwait(false);
                    var productApiResponse = JsonConvert.DeserializeObject<ApiResponse<ProductDetailResponse>>(productJson);

                    if (productApiResponse?.Data != null)
                    {
                        var product = productApiResponse.Data;
                        var productViewModel = new ProductDetailViewModel
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

                        // Get default product item (first one)
                        var defaultProductItem = productViewModel.ProductItems?.FirstOrDefault();
                        var selectedProductItemId = defaultProductItem?.Id ?? 0;
                        var price = defaultProductItem?.Price ?? productViewModel.Price;

                        var viewModel = new PaymentViewModel
                        {
                            ProductId = id,
                            Quantity = quantity,
                            Product = productViewModel,
                            SelectedProductItemId = selectedProductItemId,
                            SubTotal = price * quantity,
                            ShippingFee = 0, // Miễn phí vận chuyển
                            TotalPrice = (price * quantity) + product.PriceDiscount,
                        };

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
                this.logger.LogError(ex, "HTTP error occurred while loading product for payment");
                this.SetLoadingState(false);
                this.SetErrorState(true, "Không thể kết nối đến server. Vui lòng thử lại sau.");
                return this.RedirectToAction("Index", "Home");
            }
            catch (TaskCanceledException ex)
            {
                this.logger.LogError(ex, "Request timeout occurred while loading product for payment");
                this.SetLoadingState(false);
                this.SetErrorState(true, "Request timeout. Vui lòng thử lại sau.");
                return this.RedirectToAction("Index", "Home");
            }
            catch (JsonException ex)
            {
                this.logger.LogError(ex, "JSON deserialization error occurred while loading product for payment");
                this.SetLoadingState(false);
                this.SetErrorState(true, "Lỗi xử lý dữ liệu. Vui lòng thử lại sau.");
                return this.RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "Unexpected error occurred while loading product for payment");
                // Set error state
                this.SetLoadingState(false);
                this.SetErrorState(true, $"Đã xảy ra lỗi: {ex.Message}");
                return this.RedirectToAction("Index", "Home");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PaymentPost(Order order, ListItem listItem)
        {
            if (listItem == null || order == null)
            {
                this.TempData["AlertMessageError"] = "Thông tin đơn hàng không hợp lệ. Vui lòng thử lại.";
                return this.RedirectToAction("Index", "Home");
            }

            try
            {
                // Validate model state
                if (!this.ModelState.IsValid)
                {
                    this.TempData["AlertMessageError"] = "Thông tin đơn hàng không hợp lệ. Vui lòng kiểm tra lại các trường bắt buộc.";
                    this.TempData["id"] = listItem.ProductId;
                    this.TempData["quantity"] = listItem.Quantity;
                    return this.RedirectToAction("Payment", new { id = listItem.ProductId, quantity = listItem.Quantity });
                }

                var item = new ListItem
                {
                    ProductId = listItem.ProductId,
                    ProductItemId = listItem.ProductItemId,
                    Name = listItem.Name,
                    Price = listItem.Price,
                    Quantity = listItem.Quantity,
                    TotalPrice = listItem.TotalPrice, // Sử dụng TotalPrice từ _listItem (đã được tính từ client)
                    ProductSku = listItem.ProductSku ?? string.Empty,
                };

                var data = new Order
                {
                    FullName = order.FullName,
                    Phone = order.Phone,
                    Email = order.Email ?? string.Empty,
                    SubTotal = item.Price * item.Quantity,
                    TotalPrice = listItem.TotalPrice,
                    AddressLine = order.AddressLine,
                    Remarks = order.Remarks,
                    PaymentMethod = order.PaymentMethod,
                    ListItem =
                    {
                        item,
                    },
                };

                var jsonData = JsonConvert.SerializeObject(data);
                using var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
                using var httpClient = this.httpClientFactory.CreateClient();
                
                var uri = new Uri(ApiConstants.OrderCreate);
                var response = await httpClient.PostAsync(uri, content).ConfigureAwait(false);

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                    this.TempData["dataRes"] = responseContent;
                    this.TempData["phone"] = order.Phone;
                    this.TempData["custommerName"] = order.FullName;
                    this.TempData["addressLine"] = order.AddressLine;
                    this.TempData["email"] = order.Email;
                    this.TempData["paymentMethod"] = order.PaymentMethod;

                    return this.RedirectToAction("Index", "Home");
                }
                else
                {
                    this.logger.LogWarning("Order creation failed with status code: {StatusCode}", response.StatusCode);
                    this.TempData["AlertMessageError"] = "Đặt đơn hàng thất bại. Vui lòng kiểm tra lại thông tin.";
                    this.TempData["id"] = listItem.ProductId;
                    this.TempData["quantity"] = listItem.Quantity;

                    return this.RedirectToAction("Payment", new { id = listItem.ProductId, quantity = listItem.Quantity });
                }
            }
            catch (HttpRequestException ex)
            {
                this.logger.LogError(ex, "HTTP error occurred while creating order");
                this.TempData["AlertMessageError"] = "Không thể kết nối đến server. Vui lòng thử lại sau.";
                if (listItem != null)
                {
                    return this.RedirectToAction("Payment", new { id = listItem.ProductId, quantity = listItem.Quantity });
                }

                return this.RedirectToAction("Index", "Home");
            }
            catch (TaskCanceledException ex)
            {
                this.logger.LogError(ex, "Request timeout occurred while creating order");
                this.TempData["AlertMessageError"] = "Request timeout. Vui lòng thử lại sau.";
                if (listItem != null)
                {
                    return this.RedirectToAction("Payment", new { id = listItem.ProductId, quantity = listItem.Quantity });
                }

                return this.RedirectToAction("Index", "Home");
            }
            catch (JsonException ex)
            {
                this.logger.LogError(ex, "JSON serialization error occurred while creating order");
                this.TempData["AlertMessageError"] = "Lỗi xử lý dữ liệu. Vui lòng thử lại sau.";
                if (listItem != null)
                {
                    return this.RedirectToAction("Payment", new { id = listItem.ProductId, quantity = listItem.Quantity });
                }

                return this.RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "Unexpected error occurred while creating order");
                this.TempData["AlertMessageError"] = $"Đã xảy ra lỗi: {ex.Message}";
                if (listItem != null)
                {
                    return this.RedirectToAction("Payment", new { id = listItem.ProductId, quantity = listItem.Quantity });
                }

                return this.RedirectToAction("Index", "Home");
            }
        }
    }
}
