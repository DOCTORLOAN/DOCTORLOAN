using Microsoft.AspNetCore.Mvc;

namespace DOCTORLOAN.Controllers
{
    public class ShowRoomController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
