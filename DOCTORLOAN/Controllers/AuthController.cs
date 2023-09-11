using DOCTORLOAN.Models.Products;
using DOCTORLOAN.Models.Users;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http;
using System.Reflection;
using System.Text;

namespace DOCTORLOAN.Controllers
{
    public class AuthController : Controller
    {
        private readonly IApiService _apiService;
        private const string _baseURL = "http://ec2-18-142-136-184.ap-southeast-1.compute.amazonaws.com:7979/api";

        public AuthController(IApiService apiService)
        {
            _apiService = apiService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(User model)
        {
            if (ModelState.IsValid)
            {
                var response = await _apiService.LoginAsync(model);
                if (response.IsSuccessStatusCode)
                {
                    // Xử lý đăng nhập thành công
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    // Xử lý đăng nhập không thành công
                    ModelState.AddModelError(string.Empty, "Đăng nhập không thành công.");
                }
            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Register()
        {
            //List<Reservation> reservationList = new List<Reservation>();
            //using (var httpClient = new HttpClient())
            //{
            //    using (var response = await httpClient.GetAsync("http://ec2-18-142-136-184.ap-southeast-1.compute.amazonaws.com:7979/api/auth-module/Auth/register"))
            //    {
            //        if (response.StatusCode == System.Net.HttpStatusCode.OK)
            //        {
            //            string apiRespone = await response.Content.ReadAsStringAsync();
            //            reservationList = JsonConvert.DeserializeObject<List<Reservation>>(apiRespone);
            //        }
            //        else
            //            ViewBag.StatusCode = response.StatusCode;
            //    }
            //}
            await Task.CompletedTask;
            return View();
        }

        public IActionResult ForgotPassword()
        {
            return View();
        }
    }
}
