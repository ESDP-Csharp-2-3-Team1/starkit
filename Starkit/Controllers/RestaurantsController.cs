using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Starkit.Models;
using Starkit.Models.Data;
using Starkit.Services;



namespace Starkit.Controllers
{
    public class RestaurantsController : Controller
    {
        private StarkitContext _db { get; set; }
        private UserManager<User> _userManager { get; set; }
        private IHostEnvironment _environment { get; set; }
        private UploadService _uploadService { get; set; }

        public RestaurantsController(StarkitContext db, UserManager<User> userManager, IHostEnvironment environment, UploadService uploadService)
        {
            _db = db;
            _userManager = userManager;
            _environment = environment;
            _uploadService = uploadService;
        }


        // GET
        [Authorize]
        public async Task<IActionResult> Index()
        {
            string userId = _userManager.GetUserId(User);
            Restaurant restaurant = await _db.Restaurants.FirstOrDefaultAsync(r => r.UserId == userId);
            if (restaurant is null)
                return RedirectToAction("Register");
            return View(restaurant);
        }
        
        [Authorize]
        public async Task<IActionResult> Register(bool edit = false)
        {
            string userId = _userManager.GetUserId(User);
            ViewBag.Edit = false;
            if (edit)
            {
                ViewBag.Edit = true;
                Restaurant restaurant = await _db.Restaurants.FirstOrDefaultAsync(r => r.UserId == userId);
                return View(restaurant);
            }
            if (await _db.Restaurants.AnyAsync(r => r.UserId == userId))
                return RedirectToAction("Index");
            return View();
        }
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Register(Restaurant model)
        {
            
            if (ModelState.IsValid)
            {
                string userId = _userManager.GetUserId(User);
                model.UserId = userId;
                if (model.File != null)
                {
                    string directoryPath = Path.Combine(_environment.ContentRootPath,$"wwwroot\\images\\restaurants\\{model.Id}\\logo");
                    if (!Directory.Exists(directoryPath))
                        Directory.CreateDirectory(directoryPath);
                    await _uploadService.Upload(directoryPath,model.File.FileName,model.File);
                    model.LogoPath = $"images\\restaurants\\{model.Id}\\logo\\{model.File.FileName}";
                }
                await _db.Restaurants.AddAsync(model);
                User user = await _db.Users.FirstOrDefaultAsync(u => u.Id == userId);
                user.RestaurantId = model.Id;
                await _db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(model);
        }
        
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Edit(Restaurant model)
        {
            if (ModelState.IsValid)
            {
                string userId = _userManager.GetUserId(User);
                model.UserId = userId;
                Restaurant restaurant = await _db.Restaurants.FirstOrDefaultAsync(r => r.UserId == userId);
                if (model.File != null)
                {
                    string directoryPath = Path.Combine(_environment.ContentRootPath,$"wwwroot\\images\\restaurants\\{restaurant.Id}\\logo");
                    if (Directory.Exists(directoryPath)) System.IO.File.Delete("wwwroot/" + restaurant.LogoPath);
                    else
                        Directory.CreateDirectory(directoryPath);
                    await _uploadService.Upload(directoryPath,model.File.FileName,model.File);
                    restaurant.LogoPath = $"images\\restaurants\\{restaurant.Id}\\logo\\{model.File.FileName}";
                }
                restaurant.NameRestaurant = model.NameRestaurant;
                restaurant.PhoneNumber = model.PhoneNumber;
                restaurant.ContactPerson = model.ContactPerson;
                restaurant.Address = model.Address;
                restaurant.RestaurantInformation = model.RestaurantInformation;
                restaurant.DomainAvailability = model.DomainAvailability;
                restaurant.DomainName = model.DomainName;
                restaurant.DomainRegistrar = model.DomainRegistrar;
                restaurant.WorkSchedule = model.WorkSchedule;
                restaurant.TotalNumberSeats = model.TotalNumberSeats;
                restaurant.AvailableNumberSeats = model.AvailableNumberSeats;
                restaurant.OrderConditions = model.OrderConditions;
                _db.Restaurants.Update(restaurant);
                await _db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return RedirectToAction("Register", model);
        }
    }
}