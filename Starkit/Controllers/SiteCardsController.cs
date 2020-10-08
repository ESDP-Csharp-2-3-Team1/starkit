using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Starkit.Models;
using Starkit.Models.Data;
using Starkit.Services;

namespace Starkit.Controllers
{
    public class SiteCardsController : Controller
    {
        private StarkitContext _db { get; set; }
        private UserManager<User> _userManager { get; set; }
        private IHostEnvironment _environment { get; set; }
        private UploadService _uploadService { get; set; }

        public SiteCardsController(StarkitContext db, UserManager<User> userManager, IHostEnvironment environment, UploadService uploadService)
        {
            _db = db;
            _userManager = userManager;
            _environment = environment;
            _uploadService = uploadService;
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
        public async Task<ActionResult> SaveCarouselData(string dishNameCarousel1, string dishNameCarousel2 , string dishNameCarousel3, string dishTextCarousel1,string dishTextCarousel2, string dishTextCarousel3,IFormFile file1, IFormFile file2, IFormFile file3)
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
      
    }
}