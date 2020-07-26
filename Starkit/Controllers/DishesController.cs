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
    public class DishesController : Controller
    {
        private StarkitContext _db;
        private IHostEnvironment _environment;
        private UploadService _uploadService;
        private UserManager<User> _userManager { get; set; }

        public DishesController(StarkitContext db, IHostEnvironment environment, UploadService uploadService, UserManager<User> userManager)
        {
            _db = db;
            _environment = environment;
            _uploadService = uploadService;
            _userManager = userManager;
        }
        
        private void DeleteDishAvatar(Dish dish)
        {
            string userId = _userManager.GetUserId(User);
            string filePath = _environment.ContentRootPath + $"\\wwwroot\\images\\{userId}\\Dishes\\" + dish.Id; 
            if (Directory.Exists(filePath))
            {
                System.IO.File.Delete("wwwroot/" + dish.Avatar);
            }
        }

        private string Load(string id, IFormFile file)
        {
            string userId = _userManager.GetUserId(User);
            string path = Path.Combine(_environment.ContentRootPath + $"\\wwwroot\\images\\{userId}\\Dishes\\{id}");
            string photoPath = $"images/{userId}/Dishes/{id}/{file.FileName}";
            if (!Directory.Exists($"wwwroot/images/{userId}/Dishes/{id}"))
            {
                Directory.CreateDirectory($"wwwroot/images/{userId}/Dishes/{id}");
            }
            _uploadService.Upload(path, file.FileName, file);
            return photoPath;
        }

        [Authorize]
        [HttpGet]
        public IActionResult Create()
        {
            Dish dish = new Dish{Categories = _db.Categories.ToList()};
            return View(dish);
        }
        
        [HttpPost]
        public async Task<IActionResult> Create(Dish dish)
        {
            if (ModelState.IsValid)
            {
                dish.UserId = _userManager.GetUserId(User);
                dish.Avatar = Load(dish.Id, dish.File);
                _db.Entry(dish).State = EntityState.Added;
                await _db.SaveChangesAsync();
                return RedirectToAction("Create");
            }
            return View(dish);
        }

        [Authorize]
        [HttpGet]
        public IActionResult Edit(string id)
        {
            Dish dish = _db.Dishes.FirstOrDefault(d => d.Id == id);
            EditDishViewModel model = new EditDishViewModel
            {
                Id = id,
                Category = dish.Category,
                Name = dish.Name,
                Cost = dish.Cost,
                Description = dish.Description,
                Calorie = dish.Calorie,
                ProperNutrition = dish.ProperNutrition,
                Vegetarian = dish.Vegetarian,
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditDishViewModel model)
        {
            if (ModelState.IsValid)
            {
                Dish dish = _db.Dishes.FirstOrDefault(d => d.Id == model.Id);
                dish.Name = model.Name;
                dish.Cost = model.Cost;
                dish.Description = model.Description;
                dish.Calorie = model.Calorie;
                dish.ProperNutrition = model.ProperNutrition;
                dish.Vegetarian = model.Vegetarian;
                if (model.File != null)
                {
                    DeleteDishAvatar(dish);
                    dish.Avatar = Load(model.Id, model.File);
                }
                _db.Entry(dish).State = EntityState.Modified;
                await _db.SaveChangesAsync();
                return RedirectToAction("Index", "Menu");
            }
            return View(model);
        }
    }
}