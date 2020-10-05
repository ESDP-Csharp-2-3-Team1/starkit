using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Starkit.Models;
using Starkit.Models.Data;

namespace Starkit.Controllers
{
    public class SiteCardsController : Controller
    {
        private StarkitContext _db { get; set; }
        private UserManager<User> _userManager { get; set; }

        public SiteCardsController(StarkitContext db, UserManager<User> userManager)
        {
            _db = db;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            
            var userId = _userManager.GetUserId(User);
            ViewBag.Restaurant = await _db.Restaurants.FirstOrDefaultAsync(r => r.UserId == userId);
            return View();
        }

        public async Task<ActionResult> SiteSettings()
        {
            
        }
    }
}