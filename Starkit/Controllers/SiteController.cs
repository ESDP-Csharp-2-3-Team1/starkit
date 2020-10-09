using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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
            /// Обязательный параметр без этого редактирование сайта не будет работать 
            ViewBag.UserIsAuthenticated = User.Identity.IsAuthenticated;
            
            User user = await _db.Users.
                FirstOrDefaultAsync(u => u.Id == _userManager.GetUserId(User));
            if (User.IsInRole("SuperAdmin"))
            {
                string userId = user.IdOfTheSelectedRestaurateur;
                user = await _userManager.FindByIdAsync(userId);
            }
            Restaurant restaurant;
            var _restaurant = await _db.Restaurants.FirstOrDefaultAsync(r => r.UserId == user.Id);
            ViewBag.Restaurant = _restaurant;
            if (user != null)
            {
                restaurant = await _db.Restaurants
                    .FirstOrDefaultAsync(r => r.Id == user.RestaurantId);
            }
            
            else
            {
                string host = HttpContext.Request.Host.Value;
                restaurant = await _db.Restaurants
                    .FirstOrDefaultAsync(r => r.DomainName == host);
            }
            restaurant.DishesGroup = restaurant.Dishes.GroupBy(d => d.Category);
            ViewBag.Carousel = await _db.DataSiteCards.FirstOrDefaultAsync(d => d.RestaurantId == restaurant.Id);
            var categories = _db.Categories.Where(c=>c.RestaurantId == restaurant.Id).ToList();
            categories.Insert(0, new Category {Name = "Все", Id = null});
            ViewBag.Categories = categories;
            
            return View(restaurant);
        }

        public async Task<IActionResult> GetDishes(string id = null)
        {
            List<Dish> dishes = new List<Dish>();
            if (id != null)
            {
                 dishes = _db.Dishes.Where(d => d.CategoryId == id).ToList();
                 return PartialView("Partial/ListDishesPartialView", dishes);
            }
            else
            {
                User user = await _db.Users.
                    FirstOrDefaultAsync(u => u.Id == _userManager.GetUserId(User));
                Restaurant restaurant;
                if (user != null)
                {
                    restaurant = await _db.Restaurants
                        .FirstOrDefaultAsync(r => r.Id == user.RestaurantId);
                    return PartialView("Partial/ListDishesPartialView", restaurant.Dishes);
                }
                string host = HttpContext.Request.Host.Value;
            
                restaurant = await _db.Restaurants
                    .FirstOrDefaultAsync(r => r.DomainName == host);
                return PartialView("Partial/ListDishesPartialView", restaurant.Dishes);
            }
        }
    }
}