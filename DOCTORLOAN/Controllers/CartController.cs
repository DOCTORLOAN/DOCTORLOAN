using DOCTORLOAN.Helpers;
using DOCTORLOAN.Models.Products;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Session;
using Newtonsoft.Json;
using DOCTORLOAN.Models.Orders;
using System.Text;
using DOCTORLOAN.Models.Users;
using Microsoft.AspNetCore.Http;

namespace DOCTORLOAN.Controllers
{
    public class CartController : Controller
    {
        private readonly IHttpContextAccessor _contx;

        public CartController(IHttpContextAccessor httpContextAccessor)
        {
            _contx = httpContextAccessor;
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
            return View();
        }

        public async Task<IActionResult> PaymentPost(HttpRequest request, Order _order, ListItem _listItem)
        {
            try
            {
                string user = HttpContext.Session.GetString("Payment");
                var tmp = request;
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
