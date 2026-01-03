// <copyright file="AboutController.cs" company="DOCTORLOAN">
// Copyright (c) DOCTORLOAN. All rights reserved.
// </copyright>

using Microsoft.AspNetCore.Mvc;

namespace DOCTORLOAN.Controllers
{
    public class AboutController : Controller
    {
        public IActionResult Index()
        {
            return this.View();
        }
    }
}
