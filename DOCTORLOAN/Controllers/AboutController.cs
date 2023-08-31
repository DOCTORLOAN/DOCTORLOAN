using Microsoft.AspNetCore.Mvc;

namespace DOCTORLOAN.Controllers
{
    public class AboutController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
