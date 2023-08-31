using Microsoft.AspNetCore.Mvc;

namespace DOCTORLOAN.Controllers
{
    public class CartController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Payment()
        {
            return View();
        }
    }
}
