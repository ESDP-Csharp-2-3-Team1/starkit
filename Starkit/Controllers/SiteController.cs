using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Starkit.Models;
using Starkit.Models.Data;

namespace Starkit.Controllers
{
    public class SiteController : Controller
    {
        private StarkitContext _db;
        private UserManager<User> _userManager { get; set; }

        public SiteController(StarkitContext db, UserManager<User> userManager)
        {
            _db = db;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            User user = await _db.Users.
                FirstOrDefaultAsync(u => u.Id == _userManager.GetUserId(User));
            if (User.IsInRole("SuperAdmin"))
            {
                string userId = user.IdOfTheSelectedRestaurateur;
                user = await _userManager.FindByIdAsync(userId);
            }

            Restaurant restaurant;

            if (user != null)
            {
                restaurant = await _db.Restaurants
                    .FirstOrDefaultAsync(r => r.Id == user.RestaurantId);
                restaurant.DishesGroup = restaurant.Dishes.GroupBy(d => d.Category);
                return View(restaurant);
            }
            
            else
            {
                string host = HttpContext.Request.Host.Value;
            
                restaurant = await _db.Restaurants
                    .FirstOrDefaultAsync(r => r.DomainName == host);
                restaurant.DishesGroup = restaurant.Dishes.GroupBy(d => d.Category);
                return View(restaurant);
            }
            
            
        }
    }
}