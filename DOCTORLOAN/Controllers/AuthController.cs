using DOCTORLOAN.Models.VMAuth;
using DOCTORLOAN.service;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

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
        public async Task<IActionResult> Login(Signin modelLogin)
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
        }

        //[HttpPost]
        //public async Task<IActionResult> Register()
        //{
        //    //List<Reservation> reservationList = new List<Reservation>();
        //    //using (var httpClient = new HttpClient())
        //    //{
        //    //    using (var response = await httpClient.GetAsync("http://ec2-18-142-136-184.ap-southeast-1.compute.amazonaws.com:7979/api/auth-module/Auth/register"))
        //    //    {
        //    //        if (response.StatusCode == System.Net.HttpStatusCode.OK)
        //    //        {
        //    //            string apiRespone = await response.Content.ReadAsStringAsync();
        //    //            reservationList = JsonConvert.DeserializeObject<List<Reservation>>(apiRespone);
        //    //        }
        //    //        else
        //    //            ViewBag.StatusCode = response.StatusCode;
        //    //    }
        //    //}
        //    await Task.CompletedTask;
        //    return View();
        //}

        //public IActionResult ForgotPassword()
        //{
        //    return View();
        //}
    }
}
