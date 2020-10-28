using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Starkit.Models;
using Starkit.Models.Data;
using Starkit.Services;

namespace Starkit.Controllers
{
    public class SiteController : Controller
    {
        private StarkitContext _db;
        private UserManager<User> _userManager;
        private IHostEnvironment _environment;
        private UploadService _uploadService;
        private readonly ILogger<SiteController> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public SiteController(StarkitContext db, UserManager<User> userManager, IHostEnvironment environment, UploadService uploadService, ILogger<SiteController> logger, IHttpContextAccessor httpContextAccessor)
        {
            _db = db;
            _userManager = userManager;
            _environment = environment;
            _uploadService = uploadService;
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
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
            }
            else
            {
                string host = _httpContextAccessor.HttpContext.Request.Host.Value;
                restaurant = await _db.Restaurants
                    .FirstOrDefaultAsync(r => r.DomainName == host);
            }
            restaurant.DishesGroup = restaurant.Dishes.GroupBy(d => d.Category);
            ViewBag.Data = await _db.DataSiteCards.FirstOrDefaultAsync(d => d.RestaurantId == restaurant.Id);

            return View(restaurant);
        }
        
        public async Task<IActionResult> GetDishes(string id = null)
        {
            User user = await _db.Users.
                FirstOrDefaultAsync(u => u.Id == _userManager.GetUserId(User));
            Restaurant restaurant;
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

            List<Dish> dishes;
            if (id == null)
            {
                dishes = restaurant.Dishes;
            }
            else
            {
                dishes = restaurant.Dishes.Where(d => d.CategoryId == id).ToList();
            }
            return PartialView("Partial/ListDishesPartialView", dishes);
        }

        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<ActionResult> SaveCarouselData(string dishNameCarousel1, string dishNameCarousel2 , string dishNameCarousel3, string dishTextCarousel1,string dishTextCarousel2, string dishTextCarousel3,IFormFile file1, IFormFile file2, IFormFile file3)
        {
            string userId = _userManager.GetUserId(User);
            if (User.IsInRole(Convert.ToString(Roles.SuperAdmin)))
            {
                User admin = await _db.Users.FirstOrDefaultAsync(u => u.Id == userId);
                userId = admin.IdOfTheSelectedRestaurateur;
            }
            var restaurant = await _db.Restaurants.FirstOrDefaultAsync(r => r.UserId == userId);
            var siteData = await _db.DataSiteCards.FirstOrDefaultAsync(d => d.RestaurantId == restaurant.Id);

            siteData.DishNameCarousel1 = dishNameCarousel1;
            siteData.DishNameCarousel2 = dishNameCarousel2;
            siteData.DishNameCarousel3 = dishNameCarousel3;

            siteData.DishTextCarousel1 = dishTextCarousel1;
            siteData.DishTextCarousel2 = dishTextCarousel2;
            siteData.DishTextCarousel3 = dishTextCarousel3;

            if (file1 != null)
                siteData.ImgPathCarousel1 = await CreateFile(file1);
            if (file2 != null)
                siteData.ImgPathCarousel2 = await CreateFile(file2);
            if (file3 != null)
                siteData.ImgPathCarousel3 = await CreateFile(file3);
            
            _db.DataSiteCards.Update(siteData);
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private async Task<string> CreateFile(IFormFile file)
        {
            string directoryPath = Path.Combine(_environment.ContentRootPath,"wwwroot/images/restaurants/Carousel");
            await _uploadService.Upload(directoryPath,file.FileName,file);
            return $"images/restaurants/Carousel/{file.FileName}";
        }

        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SaveSectionData(string block, string title, string subtitle, IFormFile file)
        {
            string userId = _userManager.GetUserId(User);
            if (User.IsInRole(Convert.ToString(Roles.SuperAdmin)))
            {
                User admin = await _db.Users.FirstOrDefaultAsync(u => u.Id == userId);
                userId = admin.IdOfTheSelectedRestaurateur;
            }
            var restaurant = await _db.Restaurants.FirstOrDefaultAsync(r => r.UserId == userId);
            var siteData = await _db.DataSiteCards.FirstOrDefaultAsync(d => d.RestaurantId == restaurant.Id);
            switch (block)
            {
                case "specialOffers":
                    siteData.SpecialOffersTitle = title;
                    siteData.SpecialOffersSubtitle = subtitle;
                    if (file != null)
                        siteData.ImgPathSpecialOffers = await CreateFile(file);
                    break;
                case "menu":
                    siteData.MenuTitle = title;
                    siteData.MenuSubtitle = subtitle;
                    if (file != null)
                        siteData.ImgPathMenu = await CreateFile(file);
                    break;
                case "dishes":
                    siteData.DishesTitle = title;
                    siteData.DishesSubtitle = subtitle;
                    if (file != null)
                        siteData.ImgPathDishes = await CreateFile(file);
                    break;
                case "booking":
                    siteData.BookingTitle = title;
                    siteData.BookingSubtitle = subtitle;
                    if (file != null)
                        siteData.ImgPathBooking = await CreateFile(file);
                    break;
            }

            _db.DataSiteCards.Update(siteData);
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}