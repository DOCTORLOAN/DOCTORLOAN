using Amazon.EC2.Model;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace DOCTORLOAN.Controllers
{
    public class ProductsController : Controller
    {
        Uri baseAddress = new Uri("http://ec2-18-142-136-184.ap-southeast-1.compute.amazonaws.com:7979/api");
        private readonly HttpClient _client;

        public ProductsController() 
        {
            _client = new HttpClient();
            _client.BaseAddress = baseAddress;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            List<Reservation> reservationList = new List<Reservation>();
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync(_client.BaseAddress + "/product-module/Product/FilterProducts"));
                {
                    if (response.IsSuccessStatusCode)
                    {
                        string data = await response.Content.ReadAsStringAsync();
                        reservationList = JsonConvert.DeserializeObject<List<Reservation>>(data);
                    }
                    else
                        ViewBag.StatusCode = response.StatusCode;
                }
            }
            return View();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> ProductDetail(int id)
        {
            List<Reservation> reservationList = new List<Reservation>();
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync("http://ec2-18-142-136-184.ap-southeast-1.compute.amazonaws.com:7979/api/product-module/Product/GetProduct"))
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        string apiRespone = await response.Content.ReadAsStringAsync();
                        reservationList = JsonConvert.DeserializeObject<List<Reservation>>(apiRespone);
                    }
                    else
                        ViewBag.StatusCode = response.StatusCode;
                }
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ProductInsert()
        {
            List<Reservation> reservationList = new List<Reservation>();
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync("http://ec2-18-142-136-184.ap-southeast-1.compute.amazonaws.com:7979/api/product-module/Product/InsertProduct"))
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        string apiRespone = await response.Content.ReadAsStringAsync();
                        reservationList = JsonConvert.DeserializeObject<List<Reservation>>(apiRespone);
                    }
                    else
                        ViewBag.StatusCode = response.StatusCode;
                }
            }
            return View();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> ProductUpdate(int id)
        {
            List<Reservation> reservationList = new List<Reservation>();
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync("http://ec2-18-142-136-184.ap-southeast-1.compute.amazonaws.com:7979/api/product-module/Product/UpdateProduct"))
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        string apiRespone = await response.Content.ReadAsStringAsync();
                        reservationList = JsonConvert.DeserializeObject<List<Reservation>>(apiRespone);
                    }
                    else
                        ViewBag.StatusCode = response.StatusCode;
                }
            }
            return View();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> ProductUpdateStatus(int id)
        {
            List<Reservation> reservationList = new List<Reservation>();
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync("http://ec2-18-142-136-184.ap-southeast-1.compute.amazonaws.com:7979/api/product-module/Product/UpdateProductStatus"))
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        string apiRespone = await response.Content.ReadAsStringAsync();
                        reservationList = JsonConvert.DeserializeObject<List<Reservation>>(apiRespone);
                    }
                    else
                        ViewBag.StatusCode = response.StatusCode;
                }
            }
            return View();
        }

        //[HttpDelete("{id:int}")]
        //public async Task<ActionResult<Pro>> DeleteEmployee(int id)
        //{
        //    try
        //    {
        //        var employeeToDelete = await employeeRepository.GetEmployee(id);

        //        if (employeeToDelete == null)
        //        {
        //            return NotFound($"Employee with Id = {id} not found");
        //        }

        //        return await employeeRepository.DeleteEmployee(id);
        //    }
        //    catch (Exception)
        //    {
        //        return StatusCode(StatusCodes.Status500InternalServerError,
        //            "Error deleting data");
        //    }
        //}
    }
}
