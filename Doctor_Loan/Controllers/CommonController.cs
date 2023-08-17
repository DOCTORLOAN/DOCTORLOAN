using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Doctor_Loan.Controllers
{
    public class CommonController : Controller
    {
        // GET: Common
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult WarrantyPolicy()
        {
            return View();
        }

        public ActionResult OrderSuccess()
        {
            return View();
        }

        public ActionResult Header()
        {
            return PartialView("_Header");
        }

        public ActionResult Footer()
        {
            return PartialView("_Footer");
        }

        public ActionResult Experience()
        {
            return PartialView("_Experience");
        }
    }
}