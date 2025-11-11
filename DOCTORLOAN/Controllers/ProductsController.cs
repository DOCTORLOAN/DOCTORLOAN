using Microsoft.AspNetCore.Mvc;

namespace DOCTORLOAN.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<ProductsController> _logger;

        public ProductsController(IHttpClientFactory httpClientFactory, ILogger<ProductsController> logger)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult ProductDetail(int? productId, int? categoryId)
        {
            if ((!productId.HasValue || productId <= 0) && (!categoryId.HasValue || categoryId <= 0))
            {
                _logger.LogInformation("Truy cập ProductDetail bị từ chối do thiếu tham số hợp lệ.");
                return RedirectToAction("Index", "Home");
            }

            if (productId.HasValue && productId > 0)
            {
                ViewData["ProductId"] = productId.Value;
            }

            if (categoryId.HasValue && categoryId > 0)
            {
                ViewData["CategoryId"] = categoryId.Value;
            }

            return View();
        }
    }
}
