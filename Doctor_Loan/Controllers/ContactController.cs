using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Doctor_Loan.Controllers
{
    public class ContactController : Controller
    {
        // GET: Contact
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult MedicalRegister()
        {
            return View();
        }

        public ActionResult HealthAdvice()
        {
            return View();
        }

        public ActionResult ProductConsultation()
        {
            return View();
        }
    }
}