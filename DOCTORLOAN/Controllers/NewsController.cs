using Microsoft.AspNetCore.Mvc;

namespace DOCTORLOAN.Controllers
{
    public class NewsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult NewsDetail()
        {
            return View();
        }

        public IActionResult NewsGroup()
        {
            return View();
        }

        public IActionResult Sales()
        {
            return View();
        }
    }
}
