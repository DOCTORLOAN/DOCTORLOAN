using DOCTORLOAN.Models.Bookings;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using Newtonsoft.Json;

namespace DOCTORLOAN.Controllers
{
    public class ClinicController : Controller
    {
        private readonly HttpClient _httpClient;

        public IActionResult Index()
        {
            return View();
        }

        public ClinicController (HttpClient httpclient)
        {
            _httpClient = httpclient;
        }

        public async Task<IActionResult> Booking(Booking _booking)
        {
            try
            {
                Booking data = new Booking
                {
                    Type = 100,
                    FirstName = _booking.FirstName,
                    LastName = _booking.LastName,
                    Phone = _booking.Phone,
                    BookingDate = _booking.BookingDate,
                    AddressLine = _booking.AddressLine,
                    ProvinceId = 4,
                    DistrictId = 1,
                    WardId = 1,
                    Noted = "Đặt lịch Khám: " + _booking.Noted,
                };

                string jsonData = JsonConvert.SerializeObject(data);
                HttpContent content = new StringContent(jsonData, Encoding.UTF8, "application/json");

                HttpClient httpClient = new HttpClient();
                /*var response = await httpClient.PostAsync("http://doctorloan-api.giathaidoctorloan.vn/api/booking-module/Booking/create", content);*/
                var response = await httpClient.PostAsync("http://localhost:49553/api/booking-module/Booking/create", content);

                if (response.IsSuccessStatusCode)
                {
                    string responseContent = await response.Content.ReadAsStringAsync();
                    TempData["AlertMessageSuccess"] = "Booking thành công!";
                }
                else
                {
                    TempData["AlertMessageError"] = "Booking thất bại. vui lòng kiểm tra lại thông tin ";
                }
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

    }
}
