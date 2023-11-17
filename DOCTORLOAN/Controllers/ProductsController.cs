using Microsoft.AspNetCore.Mvc;

namespace DOCTORLOAN.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public ProductsController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IActionResult> Index()
        {
            return View();
        }

        //[HttpGet("{id}")]
        public async Task<IActionResult> ProductDetail(int id)
        {
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

        public async Task<IActionResult> ProductGCF4()
        {
            return View();
        }

        public async Task<IActionResult> ProductGCF5()
        {
            return View();
        }

        public async Task<IActionResult> ProductGCNL()
        {
            return View();
        }

        public async Task<IActionResult> ProductGCNLL()
        {
            return View();
        }

        public async Task<IActionResult> ProductGCDX()
        {
            return View();
        }

        public async Task<IActionResult> ProductGCDL()
        {
            return View();
        }

        public async Task<IActionResult> ProductGCDLKL()
        {
            return View();
        }

        public async Task<IActionResult> ProductGLF3()
        {
            return View();
        }

        public async Task<IActionResult> ProductGLF1()
        {
            return View();
        }

        public async Task<IActionResult> ProductDT()
        {
            return View();
        }
    }
}
