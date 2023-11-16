using DOCTORLOAN.Models.Bookings;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace DOCTORLOAN.Controllers
{
    public class ContactController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult MedicalRegister()
        {
            return View();
        }

        public async Task<IActionResult> MedicalRegisterPost(Booking _booking)
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
                //var response = await httpClient.PostAsync("http://dev-doctorloan-api.giathaidoctorloan.vn/api/booking-module/Booking/create", content);

                if (response.IsSuccessStatusCode)
                {
                    string responseContent = await response.Content.ReadAsStringAsync();
                    TempData["AlertMessageSuccess"] = "Booking thành công!";
                    return RedirectToAction("MedicalRegister");
                }
                else
                {
                    TempData["AlertMessageError"] = "Booking thất bại. vui lòng kiểm tra lại thông tin ";
                    return RedirectToAction("MedicalRegister");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        public IActionResult HealthAdvice()
        {
            return View();
        }

        public async Task<IActionResult> HealthAdvicePost(Booking _booking)
        {
            try
            {
                Booking data = new Booking
                {
                    Type = 10,
                    FirstName = _booking.FirstName,
                    LastName = _booking.LastName,
                    Phone = _booking.Phone,
                    BookingDate = _booking.BookingDate,
                    AddressLine = _booking.AddressLine,
                    ProvinceId = 4,
                    DistrictId = 1,
                    WardId = 1,
                    Noted = "Đăng ký tư vấn sức khoẻ: " + _booking.Noted,
                };

                string jsonData = JsonConvert.SerializeObject(data);
                HttpContent content = new StringContent(jsonData, Encoding.UTF8, "application/json");

                HttpClient httpClient = new HttpClient();
                /*var response = await httpClient.PostAsync("http://doctorloan-api.giathaidoctorloan.vn/api/booking-module/Booking/create", content);*/
                var response = await httpClient.PostAsync("http://localhost:49553/api/booking-module/Booking/create", content);
                //var response = await httpClient.PostAsync("http://dev-doctorloan-api.giathaidoctorloan.vn/api/booking-module/Booking/create", content);

                if (response.IsSuccessStatusCode)
                {
                    string responseContent = await response.Content.ReadAsStringAsync();
                    TempData["AlertMessageSuccess"] = "Booking thành công!";
                    return RedirectToAction("HealthAdvice");
                }
                else
                {
                    TempData["AlertMessageError"] = "Booking thất bại. vui lòng kiểm tra lại thông tin ";
                    return RedirectToAction("HealthAdvice");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        public IActionResult ProductConsultation()
        {
            return View();
        }

        public async Task<IActionResult> ProductConsultationPost(Booking _booking)
        {
            try
            {
                Booking data = new Booking
                {
                    Type = 20,
                    FirstName = _booking.FirstName,
                    LastName = _booking.LastName,
                    Phone = _booking.Phone,
                    BookingDate = _booking.BookingDate,
                    AddressLine = _booking.AddressLine,
                    ProvinceId = 4,
                    DistrictId = 1,
                    WardId = 1,
                    Noted = "Yêu cầu tư vấn về sản phẩm: " + _booking.Noted,
                };

                string jsonData = JsonConvert.SerializeObject(data);
                HttpContent content = new StringContent(jsonData, Encoding.UTF8, "application/json");

                HttpClient httpClient = new HttpClient();
                /*var response = await httpClient.PostAsync("http://doctorloan-api.giathaidoctorloan.vn/api/booking-module/Booking/create", content);*/
                var response = await httpClient.PostAsync("http://localhost:49553/api/booking-module/Booking/create", content);
                //var response = await httpClient.PostAsync("http://dev-doctorloan-api.giathaidoctorloan.vn/api/booking-module/Booking/create", content);

                if (response.IsSuccessStatusCode)
                {
                    string responseContent = await response.Content.ReadAsStringAsync();
                    TempData["AlertMessageSuccess"] = "Booking thành công!";
                    return RedirectToAction("ProductConsultation");
                }
                else
                {
                    TempData["AlertMessageError"] = "Booking thất bại. vui lòng kiểm tra lại thông tin ";
                    return RedirectToAction("ProductConsultation");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
