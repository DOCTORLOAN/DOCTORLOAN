using DOCTORLOAN.Models;
using DOCTORLOAN.Models.Payoo;
using DOCTORLOAN.Services;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace DOCTORLOAN.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly PayooService _payooService;

        public HomeController(ILogger<HomeController> logger, PayooService payooService)
        {
            _logger = logger;
            _payooService = payooService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        /// <summary>
        /// Xử lý thanh toán Payoo - Server-side
        /// </summary>
        [HttpPost]
        [IgnoreAntiforgeryToken] // API endpoint không cần antiforgery token
        public async Task<IActionResult> ProcessPayooPayment([FromBody] PayooPaymentRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Json(new PayooPaymentResponse
                    {
                        Success = false,
                        ErrorMessage = "Thông tin thanh toán không hợp lệ"
                    });
                }

                var result = await _payooService.CreatePaymentAsync(request);

                if (result.Success && !string.IsNullOrEmpty(result.PaymentUrl))
                {
                    return Json(result);
                }

                return Json(new PayooPaymentResponse
                {
                    Success = false,
                    ErrorMessage = result.ErrorMessage ?? "Không thể tạo đơn hàng thanh toán"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing Payoo payment");
                return Json(new PayooPaymentResponse
                {
                    Success = false,
                    ErrorMessage = "Đã xảy ra lỗi khi xử lý thanh toán. Vui lòng thử lại sau."
                });
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}