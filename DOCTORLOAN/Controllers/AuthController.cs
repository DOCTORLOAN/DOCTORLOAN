using DOCTORLOAN.Models.VMAuth;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using DOCTORLOAN.Models.Users;
using DOCTORLOAN.Models.Bookings;
using Newtonsoft.Json;
using System.Text;
using BCrypt.Net;

namespace DOCTORLOAN.Controllers
{
    public class AuthController : Controller
    {
        public IActionResult Login()
        {
            ClaimsPrincipal claimUser = HttpContext.User;

            if (claimUser.Identity.IsAuthenticated)
                return RedirectToAction("Index", "Home");


            return View();
        }

        [HttpPost]
        public async Task<IActionResult> LoginPost(Signin modelLogin)
        {
            return View();
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> RegisterPost(User _user)
        {
            try
            {
                User data = new User
                {
                    Code = "",
                    FirstName = _user.FirstName,
                    LastName = _user.LastName,
                    Password = _user.Password,
                    Email = _user.Email,
                    Phone = _user.Phone,
                    Gender = 1,
                    Avatar = 0,
                    Status = 2,
                    DOB = DateTime.Now,
                };

                string jsonData = JsonConvert.SerializeObject(data);
                HttpContent content = new StringContent(jsonData, Encoding.UTF8, "application/json");

                HttpClient httpClient = new HttpClient();
                /*var response = await httpClient.PostAsync("http://doctorloan-api.giathaidoctorloan.vn/api/booking-module/Booking/create", content);*/
                var response = await httpClient.PostAsync("http://localhost:49553/api/user-module/User/create", content);
                //var response = await httpClient.PostAsync("http://dev-doctorloan-api.giathaidoctorloan.vn/api/user-module/User/create", content);

                if (response.IsSuccessStatusCode)
                {
                    string responseContent = await response.Content.ReadAsStringAsync();
                    TempData["AlertMessageSuccess"] = "Đăng ký tài khoản thành công!";
                    return RedirectToAction("Login");
                }
                else
                {
                    TempData["AlertMessageError"] = "Đăng ký thất bại. vui lòng kiểm tra lại thông tin ";
                    return RedirectToAction("Register");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        //public IActionResult ForgotPassword()
        //{
        //    return View();
        //}
    }
}
