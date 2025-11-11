using DOCTORLOAN.Models.Orders;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace DOCTORLOAN.Controllers
{
    public class CartController : Controller
    {
        private const string OrderCreateEndpoint = "api/order-module/Order/create";
        private const string ProductEndpointTemplate = "api/product-module/Product/GetProduct?id={0}";

        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<CartController> _logger;

        public CartController(IHttpClientFactory httpClientFactory, ILogger<CartController> logger)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
        }

        public IActionResult Payment(int id, int quantity)
        {
            if (id <= 0 || quantity <= 0)
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> PaymentPost(
            [Bind("FullName,Phone,Email,AddressLine,Remarks,PaymentMethod")] Order order,
            [Bind("ProductId,ProductItemId,Name,ProductSku,OptionName,Price,Quantity")] ListItem listItem,
            CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                TempData["AlertMessageError"] = "Thông tin đơn hàng không hợp lệ, vui lòng kiểm tra lại.";
                TempData["id"] = listItem.ProductId;
                TempData["quantity"] = listItem.Quantity;
                return View("Payment");
            }

            if (!Enum.IsDefined(typeof(PaymentMethod), order.PaymentMethod))
            {
                ModelState.AddModelError(nameof(order.PaymentMethod), "Phương thức thanh toán không hợp lệ.");
            }

            if (!ModelState.IsValid)
            {
                TempData["AlertMessageError"] = "Thông tin đơn hàng không hợp lệ, vui lòng kiểm tra lại.";
                TempData["id"] = listItem.ProductId;
                TempData["quantity"] = listItem.Quantity;
                return View("Payment");
            }

            try
            {
                var client = _httpClientFactory.CreateClient("DoctorLoanApi");
                var productResponse = await client.GetAsync(string.Format(ProductEndpointTemplate, listItem.ProductId), cancellationToken);

                if (!productResponse.IsSuccessStatusCode)
                {
                    _logger.LogWarning("Không thể lấy thông tin sản phẩm {ProductId}. StatusCode: {StatusCode}", listItem.ProductId, productResponse.StatusCode);
                    TempData["AlertMessageError"] = "Không thể xác minh thông tin sản phẩm. Vui lòng thử lại sau.";
                    TempData["id"] = listItem.ProductId;
                    TempData["quantity"] = listItem.Quantity;
                    return View("Payment");
                }

                var productContent = await productResponse.Content.ReadAsStringAsync(cancellationToken);
                var productData = JsonConvert.DeserializeObject<ProductResponse>(productContent);
                if (productData?.Data == null)
                {
                    _logger.LogWarning("Dữ liệu sản phẩm rỗng cho ProductId {ProductId}", listItem.ProductId);
                    TempData["AlertMessageError"] = "Không thể xác minh thông tin sản phẩm. Vui lòng thử lại sau.";
                    TempData["id"] = listItem.ProductId;
                    TempData["quantity"] = listItem.Quantity;
                    return View("Payment");
                }

                var productItem = productData.Data.ProductItems?.FirstOrDefault(p => p.Id == listItem.ProductItemId);
                var unitPrice = productItem?.Price ?? productData.Data.Price;
                if (unitPrice <= 0)
                {
                    _logger.LogWarning("Giá sản phẩm không hợp lệ cho ProductId {ProductId}", listItem.ProductId);
                    TempData["AlertMessageError"] = "Không thể xác minh thông tin sản phẩm. Vui lòng thử lại sau.";
                    TempData["id"] = listItem.ProductId;
                    TempData["quantity"] = listItem.Quantity;
                    return View("Payment");
                }

                var totalPrice = decimal.Round(unitPrice * listItem.Quantity, 2, MidpointRounding.AwayFromZero);

                var sanitizedOrder = new Order
                {
                    FullName = order.FullName.Trim(),
                    Phone = order.Phone.Trim(),
                    Email = (order.Email ?? string.Empty).Trim(),
                    AddressLine = order.AddressLine.Trim(),
                    Remarks = order.Remarks?.Trim(),
                    PaymentMethod = order.PaymentMethod,
                    SubTotal = totalPrice,
                    TotalPrice = totalPrice,
                };

                sanitizedOrder.ListItem.Add(new ListItem
                {
                    ProductId = listItem.ProductId,
                    ProductItemId = productItem?.Id ?? listItem.ProductItemId,
                    Name = listItem.Name,
                    ProductSku = productItem?.Sku ?? listItem.ProductSku,
                    OptionName = listItem.OptionName,
                    Price = unitPrice,
                    Quantity = listItem.Quantity,
                    TotalPrice = totalPrice
                });

                var jsonData = JsonConvert.SerializeObject(sanitizedOrder);
                using var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
                var response = await client.PostAsync(OrderCreateEndpoint, content, cancellationToken);

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync(cancellationToken);

                    TempData["dataRes"] = responseContent;
                    TempData["phone"] = sanitizedOrder.Phone;
                    TempData["custommerName"] = sanitizedOrder.FullName;
                    TempData["addressLine"] = sanitizedOrder.AddressLine;
                    TempData["email"] = sanitizedOrder.Email;
                    TempData["paymentMethod"] = sanitizedOrder.PaymentMethod;

                    return RedirectToAction("Index", "Home");
                }

                _logger.LogWarning("Đặt đơn hàng thất bại với mã trạng thái {StatusCode}", response.StatusCode);
                TempData["AlertMessageError"] = "Đặt đơn hàng thất bại. Vui lòng kiểm tra lại thông tin.";
                TempData["id"] = listItem.ProductId;
                TempData["quantity"] = listItem.Quantity;
                return View("Payment");
            }
            catch (OperationCanceledException)
            {
                _logger.LogWarning("Yêu cầu đặt hàng đã bị hủy.");
                return StatusCode(499);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi trong quá trình đặt hàng.");
                return StatusCode(500, "Đã xảy ra lỗi nội bộ. Vui lòng thử lại sau.");
            }
        }

        private sealed class ProductResponse
        {
            [JsonProperty("data")]
            public ProductData? Data { get; set; }
        }

        private sealed class ProductData
        {
            [JsonProperty("price")]
            public decimal Price { get; set; }

            [JsonProperty("productItems")]
            public List<ProductItemData>? ProductItems { get; set; }
        }

        private sealed class ProductItemData
        {
            [JsonProperty("id")]
            public int Id { get; set; }

            [JsonProperty("sku")]
            public string? Sku { get; set; }

            [JsonProperty("price")]
            public decimal Price { get; set; }
        }
    }
}