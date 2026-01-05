// <copyright file="HomeController.cs" company="DOCTORLOAN">
// Copyright (c) DOCTORLOAN. All rights reserved.
// </copyright>

using System.Diagnostics;
using DOCTORLOAN.Models;
using DOCTORLOAN.Models.Payoo;
using DOCTORLOAN.Services;
using Microsoft.AspNetCore.Mvc;

namespace DOCTORLOAN.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> logger;
        private readonly PayooService payooService;

        public HomeController(ILogger<HomeController> logger, PayooService payooService)
        {
            this.logger = logger;
            this.payooService = payooService;
        }

        public IActionResult Index()
        {
            return this.View();
        }

        public IActionResult Privacy()
        {
            return this.View();
        }

        /// <summary>
        /// Xử lý thanh toán Payoo - Server-side.
        /// </summary>
        [HttpPost]
        [IgnoreAntiforgeryToken] // API endpoint không cần antiforgery token
        public async Task<IActionResult> ProcessPayooPayment([FromBody] PayooPaymentRequest request)
        {
            try
            {
                if (!this.ModelState.IsValid)
                {
                    return this.Json(new PayooPaymentResponse
                    {
                        Success = false,
                        ErrorMessage = "Thông tin thanh toán không hợp lệ",
                    });
                }

                var result = await payooService.CreatePaymentAsync(request).ConfigureAwait(false);

                if (result.Success && !string.IsNullOrEmpty(result.PaymentUrl))
                {
                    return this.Json(result);
                }

                return this.Json(new PayooPaymentResponse
                {
                    Success = false,
                    ErrorMessage = result.ErrorMessage ?? "Không thể tạo đơn hàng thanh toán",
                });
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "Error processing Payoo payment");
                return this.Json(new PayooPaymentResponse
                {
                    Success = false,
                    ErrorMessage = "Đã xảy ra lỗi khi xử lý thanh toán. Vui lòng thử lại sau.",
                });
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return this.View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? this.HttpContext.TraceIdentifier });
        }

        /// <summary>
        /// Trang 404 Not Found
        /// </summary>
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult NotFound()
        {
            Response.StatusCode = 404;
            return this.View();
        }
    }
}
