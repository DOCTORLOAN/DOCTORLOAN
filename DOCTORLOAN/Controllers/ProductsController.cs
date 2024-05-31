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

        public async Task<IActionResult> ProductDetail()
        {
            return View();
        }
    }
}
