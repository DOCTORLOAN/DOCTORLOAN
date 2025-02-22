using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Xml.Linq;

[Route("sitemap.xml")]
public class SitemapController : Controller
{
    [HttpGet]
    public IActionResult Index()
    {
        var urls = new List<string>
        {
            Url.Action("Index", "Home", null, Request.Scheme),
            Url.Action("Index", "About", null, Request.Scheme),
            Url.Action("Index", "Clinic", null, Request.Scheme),
            Url.Action("Index", "ShowRoom", null, Request.Scheme),
            Url.Action("Index", "Products", null, Request.Scheme),
            Url.Action("Index", "News", null, Request.Scheme),
            Url.Action("NewsGroup", "News", null, Request.Scheme),
            Url.Action("Index", "Contact", null, Request.Scheme),
            Url.Action("HealthAdvice", "Contact", null, Request.Scheme),
            Url.Action("MedicalRegister", "Contact", null, Request.Scheme),
            Url.Action("policydetails", "common", null, Request.Scheme),
        };

        // Tạo sitemap XML
        var sitemap = new XDocument(
            new XDeclaration("1.0", "utf-8", "yes"),
            new XElement("urlset",
                new XAttribute(XNamespace.Xmlns + "xsi", "http://www.sitemaps.org/schemas/sitemap/0.9"),
                urls.Select(url => new XElement("url",
                    new XElement("loc", url),
                    new XElement("lastmod", DateTime.UtcNow.ToString("yyyy-MM-dd")),
                    new XElement("changefreq", "daily"),
                    new XElement("priority", "0.8")
                ))
            )
        );

        return Content(sitemap.ToString(), "application/xml", Encoding.UTF8);
    }
}