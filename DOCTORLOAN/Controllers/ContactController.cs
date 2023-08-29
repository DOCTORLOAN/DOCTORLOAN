using Microsoft.AspNetCore.Mvc;

namespace DOCTORLOAN.Controllers
{
    public class ContactController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult MedicalRegister()
        {
            return View();
        }

        public IActionResult HealthAdvice()
        {
            return View();
        }

        public IActionResult ProductConsultation()
        {
            return View();
        }
    }
}
