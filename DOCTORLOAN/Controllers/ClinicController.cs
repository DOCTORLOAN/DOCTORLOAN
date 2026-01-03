// <copyright file="ClinicController.cs" company="DOCTORLOAN">
// Copyright (c) DOCTORLOAN. All rights reserved.
// </copyright>

using System.Text;
using DOCTORLOAN.Constants;
using DOCTORLOAN.Models.Bookings;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace DOCTORLOAN.Controllers
{
    public class ClinicController : Controller
    {
        private readonly IHttpClientFactory httpClientFactory;
        private readonly ILogger<ClinicController> logger;

        public ClinicController(IHttpClientFactory httpClientFactory, ILogger<ClinicController> logger)
        {
            this.httpClientFactory = httpClientFactory;
            this.logger = logger;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return this.View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Booking(Booking booking)
        {
            if (booking == null)
            {
                this.TempData["AlertMessageError"] = "Thông tin đặt lịch không hợp lệ.";
                return this.RedirectToAction("Index");
            }

            try
            {
                // Validate model state
                if (!this.ModelState.IsValid)
                {
                    this.TempData["AlertMessageError"] = "Vui lòng điền đầy đủ thông tin bắt buộc.";
                    return this.RedirectToAction("Index");
                }

                // Validate required fields
                if (string.IsNullOrWhiteSpace(booking.FirstName) ||
                    string.IsNullOrWhiteSpace(booking.LastName) ||
                    string.IsNullOrWhiteSpace(booking.Phone) ||
                    booking.BookingDate == default(DateOnly))
                {
                    this.TempData["AlertMessageError"] = "Vui lòng điền đầy đủ thông tin bắt buộc.";
                    return this.RedirectToAction("Index");
                }

                var data = new Booking
                {
                    Type = 100, // Đặt lịch Khám
                    FirstName = booking.FirstName.Trim(),
                    LastName = booking.LastName.Trim(),
                    Phone = booking.Phone.Trim(),
                    BookingDate = booking.BookingDate,
                    BookingStartTime = booking.BookingStartTime,
                    BookingEndTime = booking.BookingEndTime,
                    BookingTimes = booking.BookingTimes,
                    AddressLine = booking.AddressLine?.Trim() ?? string.Empty,
                    ProvinceId = booking.ProvinceId > 0 ? booking.ProvinceId : 4, // Default value nếu không có
                    DistrictId = booking.DistrictId > 0 ? booking.DistrictId : 1,
                    WardId = booking.WardId > 0 ? booking.WardId : 1,
                    Noted = !string.IsNullOrWhiteSpace(booking.Noted)
                        ? "Đặt lịch Khám: " + booking.Noted.Trim()
                        : "Đặt lịch Khám",
                };

                var jsonData = JsonConvert.SerializeObject(data);
                using var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

                // Sử dụng IHttpClientFactory để tạo HttpClient
                using var httpClient = this.httpClientFactory.CreateClient();
                var uri = new Uri(ApiConstants.BookingCreate);
                var response = await httpClient.PostAsync(uri, content).ConfigureAwait(false);

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    this.logger.LogInformation("Booking created successfully for phone: {Phone}", data.Phone);
                    this.TempData["AlertMessageSuccess"] = "Đặt lịch khám thành công! Chúng tôi sẽ liên hệ với bạn sớm nhất.";
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    this.logger.LogWarning(
                        "Booking failed. Status: {Status}, Response: {Response}",
                        response.StatusCode, errorContent);
                    this.TempData["AlertMessageError"] = "Đặt lịch thất bại. Vui lòng kiểm tra lại thông tin hoặc thử lại sau.";
                }

                return this.RedirectToAction("Index");
            }
            catch (HttpRequestException ex)
            {
                this.logger.LogError(ex, "HTTP error occurred while processing booking");
                this.TempData["AlertMessageError"] = "Không thể kết nối đến server. Vui lòng thử lại sau.";
                return this.RedirectToAction("Index");
            }
            catch (TaskCanceledException ex)
            {
                this.logger.LogError(ex, "Request timeout occurred while processing booking");
                this.TempData["AlertMessageError"] = "Request timeout. Vui lòng thử lại sau.";
                return this.RedirectToAction("Index");
            }
            catch (JsonException ex)
            {
                this.logger.LogError(ex, "JSON serialization error occurred while processing booking");
                this.TempData["AlertMessageError"] = "Lỗi xử lý dữ liệu. Vui lòng thử lại sau.";
                return this.RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "Unexpected error occurred while processing booking");
                this.TempData["AlertMessageError"] = "Đã xảy ra lỗi khi xử lý yêu cầu. Vui lòng thử lại sau.";
                return this.RedirectToAction("Index");
            }
        }
    }
}
