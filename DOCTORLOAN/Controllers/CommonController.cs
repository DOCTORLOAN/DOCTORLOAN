// <copyright file="CommonController.cs" company="DOCTORLOAN">
// Copyright (c) DOCTORLOAN. All rights reserved.
// </copyright>

using Microsoft.AspNetCore.Mvc;

namespace DOCTORLOAN.Controllers
{
    public class CommonController : Controller
    {
        public IActionResult Index()
        {
            return this.View();
        }

        public IActionResult PolicyDetails()
        {
            return this.View();
        }

        public IActionResult OrderSuccess()
        {
            return this.View();
        }
    }
}
