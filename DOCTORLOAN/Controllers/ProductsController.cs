using DOCTORLOAN.Models.Products;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.ComponentModel;
using System.Net.Http.Headers;
using static System.Net.WebRequestMethods;

namespace DOCTORLOAN.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private const string _baseURL = "http://localhost:49553/api";

        public ProductsController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        //[HttpGet]
        public async Task<IActionResult> Index()
        {
            //List<Product> productList = new List<Product>();
            //using (var httpClient = new HttpClient())
            //{
               
            //    try
            //    {
            //        httpClient.BaseAddress = new Uri("http://localhost:49553");
            //        httpClient.DefaultRequestHeaders.Accept.Clear();
            //        httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            //        //GET Method
            //        var response = await httpClient.GetAsync("/api/product-module/Product/GetOptions");

            //        if (response.IsSuccessStatusCode)
            //        {
            //            var data = await response.Content.ReadAsStringAsync();
            //            // a thuwr cach readFormJson xem a                       
            //            Console.WriteLine(data);
            //            return View(data);
            //        }

            //    }
            //    catch (Exception ex)
            //    {
            //        Console.WriteLine($"{ex},Internal server Error");
            //        return null;
            //    }
            //}
            return View();
        }

        ////[HttpGet]
        //public async Task<IActionResult> Index()
        //{
        //    List<Product> productList = new List<Product>();

        //    using (var httpClient = new HttpClient())
        //    {
        //        using (var response = await httpClient.GetAsync(_baseURL + "/product-module/Product/FilterProducts"))
        //        {
        //            //string apiResponse = await response.Content.ReadAsStringAsync();
        //            //productList = JsonConvert.DeserializeObject<List<Product>>(apiResponse);
        //            //Console.WriteLine(apiResponse);

        //            string apiResponse = await response.Content.ReadAsStringAsync();

        //            // Kiểm tra xem dữ liệu JSON có dạng đối tượng JSON không
        //            if (apiResponse.StartsWith("{"))
        //            {
        //                // Đối tượng JSON được trả về, deserializing thành một đối tượng Product (hoặc ViewModel)
        //                //Product product = JsonConvert.DeserializeObject<Product>(apiResponse);
        //                productList = JsonConvert.DeserializeObject<List<Product>>(apiResponse);
        //                //Console.WriteLine('product - ' + product);

        //                // Thực hiện xử lý với đối tượng Product ở đây
        //            }
        //            else if (apiResponse.StartsWith("["))
        //            {
        //                // Mảng JSON được trả về, deserializing thành danh sách sản phẩm
        //                productList = JsonConvert.DeserializeObject<List<Product>>(apiResponse);
        //                Console.WriteLine(productList);
        //                // Sử dụng danh sách sản phẩm ở đây
        //            }
        //        }
        //    }
        //    return View(productList);
        //}

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

        public async Task<IActionResult> Product135()
        {
            return View();
        }

        public async Task<IActionResult> Product90D()
        {
            return View();
        }

        public async Task<IActionResult> Product90M()
        {
            return View();
        }

        public async Task<IActionResult> Product95V()
        {
            return View();
        }

        public async Task<IActionResult> ProductN85()
        {
            return View();
        }

        public async Task<IActionResult> ProductGC()
        {
            return View();
        }

        public async Task<IActionResult> ProductGL()
        {
            return View();
        }

        public async Task<IActionResult> ProductDT()
        {
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
