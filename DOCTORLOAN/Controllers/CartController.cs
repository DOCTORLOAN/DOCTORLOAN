using DOCTORLOAN.Models.Orders;
using DOCTORLOAN.Models.Products;
using DOCTORLOAN.Models.Api;
using DOCTORLOAN.Helpers;
using DOCTORLOAN.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace DOCTORLOAN.Controllers
{
    [AllowAnonymous]
    public class CartController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public CartController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult AddToCart()
        {
            return View();
        }

        public async Task<IActionResult> Payment(int id, int quantity)
        {
            try
            {
                if (id <= 0 || quantity <= 0)
                {
                    return RedirectToAction("Index", "Home");
                }

                // Set loading state
                this.SetLoadingState(true, "Đang tải thông tin sản phẩm...");
                this.SetErrorState(false);

                var httpClient = _httpClientFactory.CreateClient();
                var productResponse = await httpClient.GetAsync($"{ApiConstants.ProductGetProduct}?id={id}");

                if (productResponse.IsSuccessStatusCode)
                {
                    var productJson = await productResponse.Content.ReadAsStringAsync();
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
                            TotalPrice = (price * quantity) + product.PriceDiscount
                        };

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

        public async Task<IActionResult> PaymentPost(Order _order, ListItem _listItem)
        {
            try
            {
                if (_listItem == null || _order == null)
                {
                    TempData["AlertMessageError"] = "Thông tin đơn hàng không hợp lệ. Vui lòng thử lại.";
                    return RedirectToAction("Index", "Home");
                }

                // Validate model state
                if (!ModelState.IsValid)
                {
                    TempData["AlertMessageError"] = "Thông tin đơn hàng không hợp lệ. Vui lòng kiểm tra lại các trường bắt buộc.";
                    TempData["id"] = _listItem.ProductId;
                    TempData["quantity"] = _listItem.Quantity;
                    return RedirectToAction("Payment", new { id = _listItem.ProductId, quantity = _listItem.Quantity });
                }

                ListItem item = new ListItem
                {
                    ProductId = _listItem.ProductId,
                    ProductItemId = _listItem.ProductItemId,
                    Name = _listItem.Name,
                    Price = _listItem.Price,
                    Quantity = _listItem.Quantity,
                    TotalPrice = _listItem.TotalPrice, // Sử dụng TotalPrice từ _listItem (đã được tính từ client)
                    ProductSku = _listItem.ProductSku ?? string.Empty,
                };

                Order data = new Order
                {
                    FullName = _order.FullName,
                    Phone = _order.Phone,
                    Email = _order.Email ?? string.Empty,
                    SubTotal = item.Price * item.Quantity,
                    TotalPrice = _listItem.TotalPrice,
                    AddressLine = _order.AddressLine,
                    Remarks = _order.Remarks,
                    PaymentMethod = _order.PaymentMethod,
                    ListItem =
                        {
                            item
                        }
                };

                string jsonData = JsonConvert.SerializeObject(data);
                HttpContent content = new StringContent(jsonData, Encoding.UTF8, "application/json");
                var httpClient = _httpClientFactory.CreateClient();
                var response = await httpClient.PostAsync(ApiConstants.OrderCreate, content);

                if (response.IsSuccessStatusCode)
                {
                    string responseContent = await response.Content.ReadAsStringAsync();

                    TempData["dataRes"] = responseContent;
                    TempData["phone"] = _order.Phone;
                    TempData["custommerName"] = _order.FullName;
                    TempData["addressLine"] = _order.AddressLine;
                    TempData["email"] = _order.Email;
                    TempData["paymentMethod"] = _order.PaymentMethod;

                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    TempData["AlertMessageError"] = "Đặt đơn hàng thất bại. Vui lòng kiểm tra lại thông tin.";
                    TempData["id"] = _listItem.ProductId;
                    TempData["quantity"] = _listItem.Quantity;

                    return RedirectToAction("Payment", new { id = _listItem.ProductId, quantity = _listItem.Quantity });
                }
            }
            catch (Exception ex)
            {
                TempData["AlertMessageError"] = $"Đã xảy ra lỗi: {ex.Message}";
                if (_listItem != null)
                {
                    return RedirectToAction("Payment", new { id = _listItem.ProductId, quantity = _listItem.Quantity });
                }
                return RedirectToAction("Index", "Home");
            }
        }
    }
}