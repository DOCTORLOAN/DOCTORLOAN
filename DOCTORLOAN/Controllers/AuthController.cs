// <copyright file="AuthController.cs" company="DOCTORLOAN">
// Copyright (c) DOCTORLOAN. All rights reserved.
// </copyright>

using System.Security.Claims;
using System.Text;
using DOCTORLOAN.Constants;
using DOCTORLOAN.Models.Users;
using DOCTORLOAN.Models.VMAuth;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace DOCTORLOAN.Controllers
{
    public class AuthController : Controller
    {
        public IActionResult Login()
        {
            ClaimsPrincipal claimUser = this.HttpContext.User;

            if (claimUser.Identity?.IsAuthenticated == true)
            {
                return this.RedirectToAction("Index", "Home");
            }

            return this.View();
        }

        /*
        public async Task<IActionResult> LoginPost(Signin modelLogin)
        {
            if (modelLogin.UserName == "admindoctorloan" &&
                modelLogin.Password == "Admin@123"
                )
            {
                List<Claim> claims = new List<Claim>() {
                    new Claim(ClaimTypes.NameIdentifier, modelLogin.UserName),
                    new Claim("OtherProperties","Example Role")

                };

                ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims,
                    CookieAuthenticationDefaults.AuthenticationScheme);

                AuthenticationProperties properties = new AuthenticationProperties()
                {
                    AllowRefresh = true,
                    IsPersistent = modelLogin.KeepLoggedIn
                };

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity), properties);

                return RedirectToAction("Index", "Home");
            }

            ViewData["ValidateMessage"] = "user not found";
            return View();
        }*/

        public IActionResult Register()
        {
            /*ClaimsPrincipal claimUser = HttpContext.User;

            if (claimUser.Identity.IsAuthenticated)
                return RedirectToAction("Index", "Home");*/

            return this.View();
        }

        /*
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
                var response = await httpClient.PostAsync(ApiConstants.BookingCreate, content);

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
        }*/

        // public IActionResult ForgotPassword()
        // {
        //    return View();
        // }
    }
}
