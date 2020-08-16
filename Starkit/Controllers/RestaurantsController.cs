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
        public IActionResult Register()
        {
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
                    string directoryPath = Path.Combine(_environment.ContentRootPath,$"wwwroot\\images\\users\\{userId}\\logo");
                    if (!Directory.Exists(directoryPath))
                        Directory.CreateDirectory(directoryPath);
                    await _uploadService.Upload(directoryPath,model.File.FileName,model.File);
                    model.LogoPath = $"images\\users\\{userId}\\logo\\{model.File.FileName}";
                }
                await _db.Restaurants.AddAsync(model);
                await _db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(model);
        }
    }
}