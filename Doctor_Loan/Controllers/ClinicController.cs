using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Doctor_Loan.Controllers
{
    public class ClinicController : Controller
    {
        // GET: Clinic
        public ActionResult Index()
        {
            return View();
        }
    }
}