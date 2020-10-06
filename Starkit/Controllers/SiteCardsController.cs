using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
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
            var restaurant = await _db.Restaurants.FirstOrDefaultAsync(r => r.UserId == userId);
            ViewBag.Restaurant = restaurant;
            ViewBag.Carousel = await _db.DataSiteCards.FirstOrDefaultAsync(d => d.RestaurantId == restaurant.Id);
            return View();
        }

        [Authorize]
        public async Task<ActionResult> SiteSettings(string restaurantId)
        {
            if (!await _db.DataSiteCards.AnyAsync(d => d.RestaurantId == restaurantId)) return View();
            {
                DataSiteCard dataSiteCard =
                    await _db.DataSiteCards.FirstOrDefaultAsync(d => d.RestaurantId == restaurantId);
                return View(dataSiteCard);
            }
        }
    }
}