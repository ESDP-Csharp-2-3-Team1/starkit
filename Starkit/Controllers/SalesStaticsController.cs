using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Starkit.Models;
using Starkit.Models.Data;

namespace Starkit.Controllers
{
    public class SalesStaticsController : Controller
    {
        private StarkitContext _db;
        private UserManager<User> _userManager;

        public SalesStaticsController(StarkitContext db, UserManager<User> userManager)
        {
            _db = db;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}