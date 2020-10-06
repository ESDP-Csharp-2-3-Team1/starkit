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
            /// Обязательный параметр без этого редактирование сайта не будет работать 
            ViewBag.UserIsAuthenticated = User.Identity.IsAuthenticated;
            
            var userId = _userManager.GetUserId(User);
            var restaurant = await _db.Restaurants.FirstOrDefaultAsync(r => r.UserId == userId);
            ViewBag.Restaurant = restaurant;
            ViewBag.Carousel = await _db.DataSiteCards.FirstOrDefaultAsync(d => d.RestaurantId == restaurant.Id);
            return View();
        }

        [Authorize]
        public async Task<ActionResult> SaveCarouselData(string dishNameCarousel1, string dishNameCarousel2 , string dishNameCarousel3, string dishTextCarousel1,string dishTextCarousel2, string dishTextCarousel3)
        {
            var userId = _userManager.GetUserId(User);
            var restaurant = await _db.Restaurants.FirstOrDefaultAsync(r => r.UserId == userId);
            var siteData = await _db.DataSiteCards.FirstOrDefaultAsync(d => d.RestaurantId == restaurant.Id);

            siteData.DishNameCarousel1 = dishNameCarousel1;
            siteData.DishNameCarousel2 = dishNameCarousel2;
            siteData.DishNameCarousel3 = dishNameCarousel3;

            siteData.DishTextCarousel1 = dishTextCarousel1;
            siteData.DishTextCarousel2 = dishTextCarousel2;
            siteData.DishTextCarousel3 = dishTextCarousel3;

            _db.DataSiteCards.Update(siteData);
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
      
    }
}