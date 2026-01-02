using DOCTORLOAN.Models.Bookings;
using DOCTORLOAN.Constants;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using Newtonsoft.Json;

namespace DOCTORLOAN.Controllers
{
    public class ClinicController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<ClinicController> _logger;

        public ClinicController(IHttpClientFactory httpClientFactory, ILogger<ClinicController> logger)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Booking(Booking _booking)
        {
            try
            {
                // Validate model state
                if (!ModelState.IsValid)
                {
                    TempData["AlertMessageError"] = "Vui lòng điền đầy đủ thông tin bắt buộc.";
                    return RedirectToAction("Index");
                }

                // Validate required fields
                if (string.IsNullOrWhiteSpace(_booking.FirstName) ||
                    string.IsNullOrWhiteSpace(_booking.LastName) ||
                    string.IsNullOrWhiteSpace(_booking.Phone) ||
                    _booking.BookingDate == default(DateOnly))
                {
                    TempData["AlertMessageError"] = "Vui lòng điền đầy đủ thông tin bắt buộc.";
                    return RedirectToAction("Index");
                }

                Booking data = new Booking
                {
                    Type = 100, // Đặt lịch Khám
                    FirstName = _booking.FirstName?.Trim() ?? string.Empty,
                    LastName = _booking.LastName?.Trim() ?? string.Empty,
                    Phone = _booking.Phone?.Trim() ?? string.Empty,
                    BookingDate = _booking.BookingDate,
                    BookingStartTime = _booking.BookingStartTime,
                    BookingEndTime = _booking.BookingEndTime,
                    BookingTimes = _booking.BookingTimes,
                    AddressLine = _booking.AddressLine?.Trim() ?? string.Empty,
                    ProvinceId = _booking.ProvinceId > 0 ? _booking.ProvinceId : 4, // Default value nếu không có
                    DistrictId = _booking.DistrictId > 0 ? _booking.DistrictId : 1,
                    WardId = _booking.WardId > 0 ? _booking.WardId : 1,
                    Noted = !string.IsNullOrWhiteSpace(_booking.Noted)
                        ? "Đặt lịch Khám: " + _booking.Noted.Trim()
                        : "Đặt lịch Khám",
                };

                string jsonData = JsonConvert.SerializeObject(data);
                HttpContent content = new StringContent(jsonData, Encoding.UTF8, "application/json");

                // Sử dụng IHttpClientFactory để tạo HttpClient
                var httpClient = _httpClientFactory.CreateClient();
                var response = await httpClient.PostAsync(ApiConstants.BookingCreate, content);

                if (response.IsSuccessStatusCode)
                {
                    string responseContent = await response.Content.ReadAsStringAsync();
                    _logger.LogInformation("Booking created successfully for phone: {Phone}", data.Phone);
                    TempData["AlertMessageSuccess"] = "Đặt lịch khám thành công! Chúng tôi sẽ liên hệ với bạn sớm nhất.";
                }
                else
                {
                    string errorContent = await response.Content.ReadAsStringAsync();
                    _logger.LogWarning("Booking failed. Status: {Status}, Response: {Response}",
                        response.StatusCode, errorContent);
                    TempData["AlertMessageError"] = "Đặt lịch thất bại. Vui lòng kiểm tra lại thông tin hoặc thử lại sau.";
                }

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while processing booking");
                TempData["AlertMessageError"] = "Đã xảy ra lỗi khi xử lý yêu cầu. Vui lòng thử lại sau.";
                return RedirectToAction("Index");
            }
        }
    }
}
