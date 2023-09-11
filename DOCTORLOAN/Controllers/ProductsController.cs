using DOCTORLOAN.Models.Products;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.ComponentModel;

namespace DOCTORLOAN.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private const string _baseURL = "http://ec2-18-142-136-184.ap-southeast-1.compute.amazonaws.com:7979/api";

        public ProductsController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            List<Product> productList = new List<Product>();

            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync(_baseURL + "/product-module/Product/FilterProducts"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    productList = JsonConvert.DeserializeObject<List<Product>>(apiResponse);
                    System.Console.WriteLine(productList);
                }
            }
            return View();
        }

        //[HttpGet("{id}")]
        public async Task<IActionResult> ProductDetail(int id)
        {
            //    List<Reservation> reservationList = new List<Reservation>();
            //    using (var httpClient = new HttpClient())
            //    {
            //        using (var response = await httpClient.GetAsync("http://ec2-18-142-136-184.ap-southeast-1.compute.amazonaws.com:7979/api/product-module/Product/GetProduct"))
            //        {
            //            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            //            {
            //                string apiRespone = await response.Content.ReadAsStringAsync();
            //                reservationList = JsonConvert.DeserializeObject<List<Reservation>>(apiRespone);
            //            }
            //            else
            //                ViewBag.StatusCode = response.StatusCode;
            //        }
            //    }
            await Task.CompletedTask;
            return View();
        }

        //[HttpPost]
        public async Task<IActionResult> ProductInsert()
        {
            //    List<Reservation> reservationList = new List<Reservation>();
            //    using (var httpClient = new HttpClient())
            //    {
            //        using (var response = await httpClient.GetAsync("http://ec2-18-142-136-184.ap-southeast-1.compute.amazonaws.com:7979/api/product-module/Product/InsertProduct"))
            //        {
            //            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            //            {
            //                string apiRespone = await response.Content.ReadAsStringAsync();
            //                reservationList = JsonConvert.DeserializeObject<List<Reservation>>(apiRespone);
            //            }
            //            else
            //                ViewBag.StatusCode = response.StatusCode;F
            //        }
            //    }
            await Task.CompletedTask;
            return View();
        }

        //[HttpPut("{id}")]
        public async Task<IActionResult> ProductUpdate(int id)
        {
            //    List<Reservation> reservationList = new List<Reservation>();
            //    using (var httpClient = new HttpClient())
            //    {
            //        using (var response = await httpClient.GetAsync("http://ec2-18-142-136-184.ap-southeast-1.compute.amazonaws.com:7979/api/product-module/Product/UpdateProduct"))
            //        {
            //            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            //            {
            //                string apiRespone = await response.Content.ReadAsStringAsync();
            //                reservationList = JsonConvert.DeserializeObject<List<Reservation>>(apiRespone);
            //            }
            //            else
            //                ViewBag.StatusCode = response.StatusCode;
            //        }
            //    }
            await Task.CompletedTask;
            return View();
        }

        //[HttpPut("{id}")]
        public async Task<IActionResult> ProductUpdateStatus(int id)
        {
            //    List<Reservation> reservationList = new List<Reservation>();
            //    using (var httpClient = new HttpClient())
            //    {
            //        using (var response = await httpClient.GetAsync("http://ec2-18-142-136-184.ap-southeast-1.compute.amazonaws.com:7979/api/product-module/Product/UpdateProductStatus"))
            //        {
            //            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            //            {
            //                string apiRespone = await response.Content.ReadAsStringAsync();
            //                reservationList = JsonConvert.DeserializeObject<List<Reservation>>(apiRespone);
            //            }
            //            else
            //                ViewBag.StatusCode = response.StatusCode;
            //        }
            //    }
            await Task.CompletedTask;
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
