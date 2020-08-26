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
using Starkit.ViewModels;

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

        private async Task<string> Load(string id, IFormFile file)
        {
            User user = await _userManager.FindByIdAsync(_userManager.GetUserId(User));
            if (User.IsInRole("SuperAdmin"))
            {
                string userId = user.IdOfTheSelectedRestaurateur;
                user = await _userManager.FindByIdAsync(userId);
            }
            string path = Path.Combine(_environment.ContentRootPath + $"\\wwwroot\\images\\restaurants\\{user.RestaurantId}\\Stocks\\{id}");
            string photoPath = $"images/restaurants/{user.RestaurantId}/Stocks/{id}/{file.FileName}";
            if (!Directory.Exists($"wwwroot/images/restaurants/{user.RestaurantId}/Stocks/{id}"))
            {
                Directory.CreateDirectory($"wwwroot/images/restaurants/{user.RestaurantId}/Stocks/{id}");
            }
            _uploadService.Upload(path, file.FileName, file);
            return photoPath;
        }
        
        private async Task DeleteStockAvatar(Stock stock)
        {
            User user = await _userManager.FindByIdAsync(_userManager.GetUserId(User));
            if (User.IsInRole("SuperAdmin"))
            {
                string userId = user.IdOfTheSelectedRestaurateur;
                user = await _userManager.FindByIdAsync(userId);
            }
            string filePath = _environment.ContentRootPath + $"\\wwwroot\\images\\restaurants\\{user.RestaurantId}\\Stocks\\" + stock.Id; 
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
        public async Task<IActionResult> Index()
        {
            User user = await _userManager.FindByIdAsync(_userManager.GetUserId(User));
            if (User.IsInRole("SuperAdmin"))
            {
                string userId = user.IdOfTheSelectedRestaurateur;
                user = await _userManager.FindByIdAsync(userId);
            }
            if (user.RestaurantId == null)
                return RedirectToAction("Register", "Restaurants");
            return View();
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            User user = await _userManager.FindByIdAsync(_userManager.GetUserId(User));
            if (User.IsInRole("SuperAdmin"))
            {
                string userId = user.IdOfTheSelectedRestaurateur;
                user = await _userManager.FindByIdAsync(userId);
            }
            if (user.RestaurantId == null)
                return RedirectToAction("Register", "Restaurants");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Stock stock)
        {
            if (ModelState.IsValid)
            {
                User user = await _userManager.FindByIdAsync(_userManager.GetUserId(User));
                if (User.IsInRole("SuperAdmin"))
                    user = await _userManager.FindByIdAsync(user.IdOfTheSelectedRestaurateur);
                stock.CreatorId = user.Id;
                stock.RestaurantId = user.RestaurantId;
                stock.Avatar = await Load(stock.Id, stock.File);
                _db.Entry(stock).State = EntityState.Added;
                await _db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(stock);
        }

        [Authorize]
        [HttpDelete]
        public async Task<IActionResult> Delete(string id)
        {
            Stock stock = new Stock{Id = id};
            _db.Entry(stock).State = EntityState.Deleted;
            await _db.SaveChangesAsync();
            DeleteStockAvatar(stock);
            return RedirectToAction("Index");
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetDishes()
        {
            User user = await _userManager.FindByIdAsync(_userManager.GetUserId(User));
            if (User.IsInRole("SuperAdmin"))
                user = await _userManager.FindByIdAsync(user.IdOfTheSelectedRestaurateur);
            List<Dish> dishes = _db.Dishes.Where(d => d.RestaurantId == user.RestaurantId).ToList();
            var dishGroup = dishes.GroupBy(d => d.Category);
            return PartialView("PartilaViews/AddDishesToStockModalWindow", dishGroup);
        }

        [Authorize]
        [HttpGet]
        public IActionResult Edit(string id)
        {
            Stock stock = _db.Stocks.FirstOrDefault(s => s.Id == id);
            EditStockViewModel model = new EditStockViewModel
            {
                Id = id,
                Name = stock.Name,
                Type = stock.Type,
                Description = stock.Description,
                Validity = stock.Validity,
                At = stock.At,
                To = stock.To,
                FirstDishId = stock.FirstDishId,
                SecondDishId = stock.SecondDishId,
                ThirdDishId = stock.ThirdDishId
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditStockViewModel model)
        {
            if (ModelState.IsValid)
            {
                User user = await _userManager.FindByIdAsync(_userManager.GetUserId(User));
                if (User.IsInRole("SuperAdmin"))
                {
                    user = await _userManager.FindByIdAsync(user.IdOfTheSelectedRestaurateur);
                }
                Stock stock = _db.Stocks.FirstOrDefault(s => s.Id == model.Id);
                stock.Name = model.Name;
                stock.Type = model.Type;
                stock.Description = model.Description;
                stock.Validity = model.Validity;
                stock.At = model.At;
                stock.To = model.To;
                stock.FirstDishId = model.FirstDishId;
                stock.SecondDishId = model.SecondDishId;
                stock.ThirdDishId = model.ThirdDishId;
                stock.EditorId = user.Id;
                if (model.File != null)
                {
                    DeleteStockAvatar(stock);
                    stock.Avatar = await Load(model.Id, model.File);
                }
                _db.Entry(stock).State = EntityState.Modified;
                await _db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(model);
        }

        [Authorize]
        [HttpGet]
        public IActionResult Details(string id)
        {
            Stock stock = _db.Stocks.FirstOrDefault(s => s.Id == id);
            return PartialView("PartilaViews/DetailStockPartialView", stock);
        }

        public async Task<IActionResult> GetStocks()
        {
            User user = await _userManager.FindByIdAsync(_userManager.GetUserId(User));
            if (User.IsInRole("SuperAdmin"))
                user = await _userManager.FindByIdAsync(user.IdOfTheSelectedRestaurateur);
            List<Stock> stocks = _db.Stocks.Where(s => s.RestaurantId == user.RestaurantId).ToList();
            return PartialView("PartilaViews/ListStockPartialView", stocks);
        }
    }
}