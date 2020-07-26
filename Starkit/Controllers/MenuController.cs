using System.Linq;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Starkit.Models;
using Starkit.Models.Data;

namespace Starkit.Controllers
{
    public class MenuController : Controller
    {
        private StarkitContext _db;
        private UserManager<User> _userManager { get; set; }

        public MenuController(StarkitContext db, UserManager<User> userManager)
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