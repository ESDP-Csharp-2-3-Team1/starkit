using Microsoft.AspNetCore.Mvc;

namespace Starkit.Controllers
{
    public class StarkitController : Controller
    {
        // GET
        public IActionResult Index()
        {
            return View();
        }
    }
}