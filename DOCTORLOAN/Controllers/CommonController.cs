using Microsoft.AspNetCore.Mvc;

namespace DOCTORLOAN.Controllers
{
    public class CommonController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult WarrantyPolicy()
        {
            return View();
        }

        public IActionResult OrderSuccess()
        {
            return View();
        }
    }
}
