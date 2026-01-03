// <copyright file="ContactController.cs" company="DOCTORLOAN">
// Copyright (c) DOCTORLOAN. All rights reserved.
// </copyright>

using System.Net;
using System.Text;
using DOCTORLOAN.Constants;
using DOCTORLOAN.Models.Bookings;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace DOCTORLOAN.Controllers
{
    public class ContactController : Controller
    {
        private readonly IHttpClientFactory httpClientFactory;
        private readonly ILogger<ContactController> logger;

        public ContactController(IHttpClientFactory httpClientFactory, ILogger<ContactController> logger)
        {
            this.httpClientFactory = httpClientFactory;
            this.logger = logger;
        }

        public IActionResult Index()
        {
            return this.View();
        }

        public IActionResult MedicalRegister()
        {
            return this.View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MedicalRegisterPost(Booking booking)
        {
            if (booking == null)
            {
                this.TempData["AlertMessageError"] = "Thông tin đặt lịch không hợp lệ.";
                return this.RedirectToAction("MedicalRegister");
            }

            try
            {
                var data = new Booking
                {
                    Type = 100,
                    FirstName = booking.FirstName,
                    LastName = booking.LastName,
                    Phone = booking.Phone,
                    BookingDate = booking.BookingDate,
                    AddressLine = booking.AddressLine,
                    ProvinceId = 4,
                    DistrictId = 1,
                    WardId = 1,
                    Noted = "Đặt lịch Khám: " + booking.Noted,
                };

                var jsonData = JsonConvert.SerializeObject(data);
                using var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
                using var httpClient = this.httpClientFactory.CreateClient();
                
                var uri = new Uri(ApiConstants.BookingCreate);
                var response = await httpClient.PostAsync(uri, content).ConfigureAwait(false);

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    this.TempData["AlertMessageSuccess"] = "Booking thành công!";
                    return this.RedirectToAction("MedicalRegister");
                }
                else
                {
                    this.logger.LogWarning("Booking failed with status code: {StatusCode}", response.StatusCode);
                    this.TempData["AlertMessageError"] = "Booking thất bại. Vui lòng kiểm tra lại thông tin.";
                    return this.RedirectToAction("MedicalRegister");
                }
            }
            catch (HttpRequestException ex)
            {
                this.logger.LogError(ex, "HTTP error occurred while creating booking");
                this.TempData["AlertMessageError"] = "Không thể kết nối đến server. Vui lòng thử lại sau.";
                return this.RedirectToAction("MedicalRegister");
            }
            catch (TaskCanceledException ex)
            {
                this.logger.LogError(ex, "Request timeout occurred while creating booking");
                this.TempData["AlertMessageError"] = "Request timeout. Vui lòng thử lại sau.";
                return this.RedirectToAction("MedicalRegister");
            }
            catch (JsonException ex)
            {
                this.logger.LogError(ex, "JSON serialization error occurred while creating booking");
                this.TempData["AlertMessageError"] = "Lỗi xử lý dữ liệu. Vui lòng thử lại sau.";
                return this.RedirectToAction("MedicalRegister");
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "Unexpected error occurred while creating booking");
                this.TempData["AlertMessageError"] = "Đã xảy ra lỗi. Vui lòng thử lại sau.";
                return this.RedirectToAction("MedicalRegister");
            }
        }

        public IActionResult HealthAdvice()
        {
            return this.View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> HealthAdvicePost(Booking booking)
        {
            if (booking == null)
            {
                this.TempData["AlertMessageError"] = "Thông tin đặt lịch không hợp lệ.";
                return this.RedirectToAction("HealthAdvice");
            }

            try
            {
                var data = new Booking
                {
                    Type = 10,
                    FirstName = booking.FirstName,
                    LastName = booking.LastName,
                    Phone = booking.Phone,
                    BookingDate = booking.BookingDate,
                    AddressLine = booking.AddressLine,
                    ProvinceId = 4,
                    DistrictId = 1,
                    WardId = 1,
                    Noted = "Đăng ký tư vấn sức khoẻ: " + booking.Noted,
                };

                var jsonData = JsonConvert.SerializeObject(data);
                using var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
                using var httpClient = this.httpClientFactory.CreateClient();
                
                var uri = new Uri(ApiConstants.BookingCreate);
                var response = await httpClient.PostAsync(uri, content).ConfigureAwait(false);

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    this.TempData["AlertMessageSuccess"] = "Booking thành công!";
                    return this.RedirectToAction("HealthAdvice");
                }
                else
                {
                    this.logger.LogWarning("Booking failed with status code: {StatusCode}", response.StatusCode);
                    this.TempData["AlertMessageError"] = "Booking thất bại. Vui lòng kiểm tra lại thông tin.";
                    return this.RedirectToAction("HealthAdvice");
                }
            }
            catch (HttpRequestException ex)
            {
                this.logger.LogError(ex, "HTTP error occurred while creating booking");
                this.TempData["AlertMessageError"] = "Không thể kết nối đến server. Vui lòng thử lại sau.";
                return this.RedirectToAction("HealthAdvice");
            }
            catch (TaskCanceledException ex)
            {
                this.logger.LogError(ex, "Request timeout occurred while creating booking");
                this.TempData["AlertMessageError"] = "Request timeout. Vui lòng thử lại sau.";
                return this.RedirectToAction("HealthAdvice");
            }
            catch (JsonException ex)
            {
                this.logger.LogError(ex, "JSON serialization error occurred while creating booking");
                this.TempData["AlertMessageError"] = "Lỗi xử lý dữ liệu. Vui lòng thử lại sau.";
                return this.RedirectToAction("HealthAdvice");
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "Unexpected error occurred while creating booking");
                this.TempData["AlertMessageError"] = "Đã xảy ra lỗi. Vui lòng thử lại sau.";
                return this.RedirectToAction("HealthAdvice");
            }
        }

        public IActionResult ProductConsultation()
        {
            return this.View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ProductConsultationPost(Booking booking)
        {
            if (booking == null)
            {
                this.TempData["AlertMessageError"] = "Thông tin đặt lịch không hợp lệ.";
                return this.RedirectToAction("ProductConsultation");
            }

            try
            {
                var data = new Booking
                {
                    Type = 20,
                    FirstName = booking.FirstName,
                    LastName = booking.LastName,
                    Phone = booking.Phone,
                    BookingDate = booking.BookingDate,
                    AddressLine = booking.AddressLine,
                    ProvinceId = 4,
                    DistrictId = 1,
                    WardId = 1,
                    Noted = "Yêu cầu tư vấn về sản phẩm: " + booking.Noted,
                };

                var jsonData = JsonConvert.SerializeObject(data);
                using var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
                using var httpClient = this.httpClientFactory.CreateClient();
                
                var uri = new Uri(ApiConstants.BookingCreate);
                var response = await httpClient.PostAsync(uri, content).ConfigureAwait(false);

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    this.TempData["AlertMessageSuccess"] = "Booking thành công!";
                    return this.RedirectToAction("ProductConsultation");
                }
                else
                {
                    this.logger.LogWarning("Booking failed with status code: {StatusCode}", response.StatusCode);
                    this.TempData["AlertMessageError"] = "Booking thất bại. Vui lòng kiểm tra lại thông tin.";
                    return this.RedirectToAction("ProductConsultation");
                }
            }
            catch (HttpRequestException ex)
            {
                this.logger.LogError(ex, "HTTP error occurred while creating booking");
                this.TempData["AlertMessageError"] = "Không thể kết nối đến server. Vui lòng thử lại sau.";
                return this.RedirectToAction("ProductConsultation");
            }
            catch (TaskCanceledException ex)
            {
                this.logger.LogError(ex, "Request timeout occurred while creating booking");
                this.TempData["AlertMessageError"] = "Request timeout. Vui lòng thử lại sau.";
                return this.RedirectToAction("ProductConsultation");
            }
            catch (JsonException ex)
            {
                this.logger.LogError(ex, "JSON serialization error occurred while creating booking");
                this.TempData["AlertMessageError"] = "Lỗi xử lý dữ liệu. Vui lòng thử lại sau.";
                return this.RedirectToAction("ProductConsultation");
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "Unexpected error occurred while creating booking");
                this.TempData["AlertMessageError"] = "Đã xảy ra lỗi. Vui lòng thử lại sau.";
                return this.RedirectToAction("ProductConsultation");
            }
        }
    }
}
