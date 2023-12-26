using DOCTORLOAN.Models.Orders;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.IO;
using System.Text;

namespace DOCTORLOAN.Controllers
{
    public class CartController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

       /* public async Task<IActionResult> AddToCart(int productId, string productName, decimal price, int quantity)
        {
            var cart = HttpContext.Session.GetObject<List<CartItem>>("Cart") ?? new List<CartItem>();

            // Kiểm tra xem sản phẩm đã tồn tại trong giỏ hàng chưa
            var existingItem = cart.FirstOrDefault(item => item.ProductId == productId);

            if (existingItem != null)
            {
                // Nếu đã tồn tại, cập nhật số lượng
                existingItem.Quantity += quantity;
            }
            else
            {
                // Nếu chưa tồn tại, thêm sản phẩm mới vào giỏ hàng
                cart.Add(new CartItem
                {
                    ProductId = productId,
                    Name = productName,
                    Price = price,
                    Quantity = quantity
                });
            }

            // Lưu giỏ hàng vào Session
            HttpContext.Session.SetObject("Cart", cart);

            // Trả về một JSON object để xử lý trên phía client nếu cần
            return Json(new { success = true });
        }*/

        public async Task<IActionResult> Payment(int id, int quantity)
        {
            return View();
        }

        public async Task<IActionResult> PaymentPost(Order _order, ListItem _listItem)
        {
            try
            {
                ListItem item = new ListItem
                {
                    ProductItemId = _listItem.ProductItemId,
                    Name = _listItem.Name,
                    Price = _listItem.Price,
                    Quantity = _listItem.Quantity,
                    TotalPrice = _order.TotalPrice,
                    ProductSku = "",
                };

                if (_order.Email == null ) {
                    _order.Email = "";
                }

                Order data = new Order
                {
                    FullName = _order.FullName,
                    Phone = _order.Phone,
                    Email = _order.Email,
                    TotalPrice = _listItem.TotalPrice,
                    AddressLine = _order.AddressLine,
                    Remarks = _order.Remarks,
                    PaymentMethod = _order.PaymentMethod,
                    ListItem  =
                    {
                        item
                    } 
                };

                string jsonData = JsonConvert.SerializeObject(data);
                HttpContent content = new StringContent(jsonData, Encoding.UTF8, "application/json");
                HttpClient httpClient = new HttpClient();
                var response = await httpClient.PostAsync("https://doctorloan-api.giathaidoctorloan.vn/api/order-module/Order/create", content);

                if (response.IsSuccessStatusCode)
                {
                    string responseContent = await response.Content.ReadAsStringAsync();

                    //send Email for Custommer
                    if (_order.Email != null)
                    {
                        string contentCustomer = "";

                    }

                    TempData["AlertMessageSuccess"] = "Đặt đơn hàng thành công!";
                    return RedirectToAction("Index", "Products");
                }
                else
                {
                    TempData["AlertMessageError"] = "Đặn đơn hàng thất bại. vui lòng kiểm tra lại thông tin ";
                    return View("Payment");
                }
                
            } catch (Exception ex) {
                return StatusCode(500, ex.Message); 
            }
        }
    } 
}
