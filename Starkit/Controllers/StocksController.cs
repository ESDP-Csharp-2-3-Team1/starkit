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
        public IActionResult Index()
        {
            return View();
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
            List<Stock> stocks = _db.Stocks.Where(s => s.CreatorId == _userManager.GetUserId(User)).ToList();
            return PartialView("PartilaViews/ListStockPartialView", stocks);
        }

        [Authorize]
        [HttpGet]
        public IActionResult GetDishes()
        {
            List<Dish> dishes = _db.Dishes.Where(d => d.CreatorId == _userManager.GetUserId(User)).ToList();
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
                if (model.File != null)
                {
                    DeleteStockAvatar(stock);
                    stock.Avatar = Load(model.Id, model.File);
                }
                _db.Entry(stock).State = EntityState.Modified;
                await _db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(model);
        }

        public IActionResult GetStocks()
        {
            List<Stock> stocks = _db.Stocks.Where(s => s.CreatorId == _userManager.GetUserId(User)).ToList();
            return PartialView("PartilaViews/ListStockPartialView", stocks);
        }
    }
}