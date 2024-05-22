using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Hosting.Server;
using System.IO;
using System;

namespace DOCTORLOAN.Controllers
{
    public class PayooController : Controller
    {
        public IActionResult Redirect()
        {
            return View();
        }
    }
}
