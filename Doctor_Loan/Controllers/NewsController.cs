using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Doctor_Loan.Controllers
{
    public class NewsController : Controller
    {
        // GET: News
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult NewsDetail()
        {
            return View();
        }

        public ActionResult NewsGroup()
        {
            return View();
        }
    }
}