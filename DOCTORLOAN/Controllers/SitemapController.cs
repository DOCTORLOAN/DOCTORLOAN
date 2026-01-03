// <copyright file="SitemapController.cs" company="DOCTORLOAN">
// Copyright (c) DOCTORLOAN. All rights reserved.
// </copyright>

using System.Text;
using System.Xml.Linq;
using Microsoft.AspNetCore.Mvc;

[Route("sitemap.xml")]
public class SitemapController : Controller
{
    [HttpGet]
    public IActionResult Index()
    {
        var urls = new List<(string Action, string Controller, string ChangeFreq, string Priority)>
        {
            ("Index", "Home", "daily", "1.0"),
            ("Index", "About", "monthly", "0.8"),
            ("Index", "Clinic", "weekly", "0.7"),
            ("Index", "ShowRoom", "weekly", "0.7"),
            ("Index", "Products", "daily", "0.9"),
            ("Index", "News", "daily", "0.9"),
            ("NewsGroup", "News", "daily", "0.9"),
            ("Index", "Contact", "monthly", "0.5"),
            ("HealthAdvice", "Contact", "daily", "0.6"),
            ("MedicalRegister", "Contact", "monthly", "0.6"),
            ("policydetails", "common", "monthly", "0.6"),
        };

        var baseUrl = $"{this.Request.Scheme}://{this.Request.Host}";
        var validUrls = urls
            .Select(u => (this.Url.Action(u.Action, u.Controller, null, this.Request.Scheme), u.ChangeFreq, u.Priority))
            .Where(u => !string.IsNullOrEmpty(u.Item1)) // Loại bỏ URL null
            .Select(u => (new Uri(new Uri(baseUrl), u.Item1).ToString(), u.ChangeFreq, u.Priority)) // Chuẩn hóa đường dẫn
            .Distinct() // Loại bỏ trùng lặp
            .ToList();

        // Dùng StringBuilder để cải thiện hiệu suất
        var sb = new StringBuilder();
        sb.AppendLine("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
        sb.AppendLine("<urlset xmlns=\"http://www.sitemaps.org/schemas/sitemap/0.9\">");

        foreach (var (loc, changeFreq, priority) in validUrls)
        {
            sb.AppendLine("  <url>");
            sb.AppendLine($"    <loc>{loc}</loc>");
            sb.AppendLine($"    <lastmod>{DateTime.UtcNow:yyyy-MM-dd}</lastmod>");
            sb.AppendLine($"    <changefreq>{changeFreq}</changefreq>");
            sb.AppendLine($"    <priority>{priority}</priority>");
            sb.AppendLine("  </url>");
        }

        sb.AppendLine("</urlset>");

        return this.Content(sb.ToString(), "application/xml", Encoding.UTF8);
    }
}
