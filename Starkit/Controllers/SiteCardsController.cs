using Microsoft.AspNetCore.Mvc;

namespace Starkit.Controllers
{
    public class SiteCardsController : Controller
    {
        
        
        // GET
        public IActionResult Index()
        {
            return View();
        }
    }
}