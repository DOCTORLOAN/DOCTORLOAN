using Microsoft.AspNetCore.Mvc;

namespace DOCTORLOAN.Controllers
{
    public class ClinicController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
