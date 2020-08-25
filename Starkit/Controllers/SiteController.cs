using Microsoft.AspNetCore.Mvc;

namespace Starkit.Controllers
{
    public class SiteController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}