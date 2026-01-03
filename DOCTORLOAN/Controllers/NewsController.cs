// <copyright file="NewsController.cs" company="DOCTORLOAN">
// Copyright (c) DOCTORLOAN. All rights reserved.
// </copyright>

using Microsoft.AspNetCore.Mvc;

namespace DOCTORLOAN.Controllers
{
    public class NewsController : Controller
    {
        public IActionResult Index()
        {
            return this.View();
        }

        public IActionResult NewsDetail()
        {
            return this.View();
        }

        /*public IActionResult NewsGroup()
        {
            return View();
        }*/
    }
}
