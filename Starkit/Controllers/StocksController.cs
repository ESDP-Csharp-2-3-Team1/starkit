using System.Collections.Generic;
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
    public class StocksController : Controller
    {
        private StarkitContext _db;
        private UserManager<User> _userManager { get; set; }
        private IHostEnvironment _environment;
        private UploadService _uploadService;

        public StocksController(StarkitContext db, IHostEnvironment environment, UploadService uploadService, UserManager<User> userManager)
        {
            _db = db;
            _environment = environment;
            _uploadService = uploadService;
            _userManager = userManager;
        }

        private string Load(string id, IFormFile file)
        {
            string userId = _userManager.GetUserId(User);
            string path = Path.Combine(_environment.ContentRootPath + $"\\wwwroot\\images\\users\\{userId}\\Stocks\\{id}");
            string photoPath = $"images/users/{userId}/Stocks/{id}/{file.FileName}";
            if (!Directory.Exists($"wwwroot/images/users/{userId}/Stocks/{id}"))
            {
                Directory.CreateDirectory($"wwwroot/images/users/{userId}/Stocks/{id}");
            }
            _uploadService.Upload(path, file.FileName, file);
            return photoPath;
        }
        
        private void DeleteStockAvatar(Stock stock)
        {
            string userId = _userManager.GetUserId(User);
            string filePath = _environment.ContentRootPath + $"\\wwwroot\\images\\users\\{userId}\\Stocks\\" + stock.Id; 
            if (Directory.Exists(filePath))
            {
                if (stock.Avatar == null)
                    Directory.Delete(filePath, true);
                else
                    System.IO.File.Delete("wwwroot/" + stock.Avatar);
            }
        }

        [Authorize]
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Stock stock)
        {
            if (ModelState.IsValid)
            {
                stock.CreatorId = _userManager.GetUserId(User);
                stock.Avatar = Load(stock.Id, stock.File);
                _db.Entry(stock).State = EntityState.Added;
                await _db.SaveChangesAsync();
                return RedirectToAction("Create");
            }
            return View(stock);
        }

        [Authorize]
        [HttpGet]
        public IActionResult GetDishes()
        {
            List<Dish> dishes = _db.Dishes.Where(d => d.CreatorId == _userManager.GetUserId(User)).ToList();
            var dishGroup = dishes.GroupBy(d => d.Category);
            return PartialView("PartilaViews/AddDishesToStockModalWindow", dishGroup);
        }
    }
}