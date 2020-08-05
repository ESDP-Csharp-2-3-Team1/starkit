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
using Starkit.Models;
using Starkit.Models.Data;
using Starkit.Services;
using Starkit.ViewModels;
using X.PagedList;

namespace Starkit.Controllers
{
    public class DishesController : Controller
    {
        private readonly StarkitContext _db;
        private readonly IHostEnvironment _environment;
        private readonly UploadService _uploadService;
        public int pageSize = 5;

        public DishesController(StarkitContext db, IHostEnvironment environment, UploadService uploadService,
            UserManager<User> userManager)
        {
            _db = db;
            _environment = environment;
            _uploadService = uploadService;
            _userManager = userManager;
        }

        private UserManager<User> _userManager { get; }

        private void DeleteDishAvatar(Dish dish)
        {
            var userId = _userManager.GetUserId(User);
            var filePath = _environment.ContentRootPath + $"\\wwwroot\\images\\{userId}\\Dishes\\" + dish.Id;
            if (Directory.Exists(filePath)) System.IO.File.Delete("wwwroot/" + dish.Avatar);
        }

        private string Load(string id, IFormFile file)
        {
            var userId = _userManager.GetUserId(User);
            var path = Path.Combine(_environment.ContentRootPath + $"\\wwwroot\\images\\{userId}\\Dishes\\{id}");
            var photoPath = $"images/{userId}/Dishes/{id}/{file.FileName}";
            if (!Directory.Exists($"wwwroot/images/{userId}/Dishes/{id}"))
                Directory.CreateDirectory($"wwwroot/images/{userId}/Dishes/{id}");
            _uploadService.Upload(path, file.FileName, file);
            return photoPath;
        }

        [Authorize]
        public IActionResult Index()
        {
            var categories = _db.Categories.ToList();
            categories.Insert(0, new Category {Name = "Все", Id = null});
            var selectList = new SelectList(categories, "Id", "Name");
            return View(selectList);
        }

        [Authorize]
        [HttpGet]
        public IActionResult Create()
        {
            var dish = new Dish {Categories = _db.Categories.ToList()};
            return View(dish);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Dish dish)
        {
            if (ModelState.IsValid)
            {
                dish.CreatorId = _userManager.GetUserId(User);
                dish.Avatar = Load(dish.Id, dish.File);
                dish.AddTime = DateTime.Now;
                _db.Entry(dish).State = EntityState.Added;
                await _db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(dish);
        }

        [Authorize]
        [HttpGet]
        public IActionResult Edit(string id)
        {
            var dish = _db.Dishes.FirstOrDefault(d => d.Id == id);
            var model = new EditDishViewModel
            {
                Id = id,
                Category = dish.Category,
                Name = dish.Name,
                Cost = dish.Cost,
                Description = dish.Description,
                Calorie = dish.Calorie,
                ProperNutrition = dish.ProperNutrition,
                Vegetarian = dish.Vegetarian,
                Ingredients = dish.Ingredients
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditDishViewModel model)
        {
            if (ModelState.IsValid)
            {
                var dish = _db.Dishes.FirstOrDefault(d => d.Id == model.Id);
                dish.Name = model.Name;
                dish.Cost = model.Cost;
                dish.Description = model.Description;
                dish.Calorie = model.Calorie;
                dish.ProperNutrition = model.ProperNutrition;
                dish.Vegetarian = model.Vegetarian;
                dish.Ingredients = model.Ingredients;
                dish.EditTime = DateTime.Now;
                dish.EditorId = _userManager.GetUserId(User);
                if (model.File != null)
                {
                    DeleteDishAvatar(dish);
                    dish.Avatar = Load(model.Id, model.File);
                }

                _db.Entry(dish).State = EntityState.Modified;
                await _db.SaveChangesAsync();
                return RedirectToAction("Index", "Dishes");
            }

            return View(model);
        }

        public async Task<IActionResult> GetDishes(string category, string name, int page = 1, SortState sortOrder = SortState.AddTimeAsc)
        {
            var dishes = _db.Dishes.Where(d => d.CreatorId == _userManager.GetUserId(User));

            if (category != null)
                dishes = dishes.Where(d => d.CategoryId == category);
            if (!string.IsNullOrEmpty(name))
                dishes = dishes.Where(d => d.Name.ToLower().Contains(name.ToLower()));

            switch (sortOrder)
            {
                case SortState.NameDesc:
                    dishes = dishes.OrderByDescending(d => d.Name);
                    break;
                case SortState.CostAsc:
                    dishes = dishes.OrderBy(d => d.Cost);
                    break;
                case SortState.CostDesc:
                    dishes = dishes.OrderByDescending(d => d.Cost);
                    break;
                case SortState.CategoryAsc:
                    dishes = dishes.OrderBy(d => d.Category);
                    break;
                case SortState.CategoryDesc:
                    dishes = dishes.OrderByDescending(d => d.Category);
                    break;
                case SortState.CalorieAsc:
                    dishes = dishes.OrderBy(d => d.Calorie);
                    break;
                case SortState.CalorieDesc:
                    dishes = dishes.OrderByDescending(d => d.Calorie);
                    break;
                case SortState.AddTimeAsc:
                    dishes = dishes.OrderBy(d => d.AddTime);
                    break;
                case SortState.AddTimeDesc:
                    dishes = dishes.OrderByDescending(d => d.AddTime);
                    break;
                default:
                    dishes = dishes.OrderBy(d => d.Name);
                    break;
            }

            var count = await dishes.CountAsync();
            var items = await dishes.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

            if (items.Count == 0 && page != 1)
            {
                page = page - 1;
                items = await dishes.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
            }

            var viewModel = new IndexViewModel
            {
                PageViewModel = new PageViewModel(count, page, pageSize),
                SortViewModel = new SortViewModel(sortOrder),
                FilterViewModel = new FilterViewModel(_db.Categories.ToList(), category, name),
                Dishes = items
            };

            return PartialView("PartialViews/LIstDishPartialView", viewModel);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(string[] ids)
        {
            var dishes = new List<Dish>();
            foreach (var id in ids)
            {
                dishes.Add(_db.Dishes.FirstOrDefault(d => d.Id == id));
                var userId = _userManager.GetUserId(User);
                var filePath = _environment.ContentRootPath + $"\\wwwroot\\images\\{userId}\\Dishes\\" + id;
                if (Directory.Exists(filePath))
                    Directory.Delete(filePath, true);
            }

            if (dishes.Count == 1)
                _db.Dishes.Remove(dishes[0]);
            else
                _db.Dishes.RemoveRange(dishes);
            await _db.SaveChangesAsync();
            return Json(true);
        }

        [HttpPut]
        public async Task<IActionResult> Hide(List<string> ids)
        {
            var dish = new Dish();
            foreach (var id in ids)
            {
                dish = _db.Dishes.FirstOrDefault(d => d.Id == id);
                if (dish.Visibility)
                    dish.Visibility = false;
                else
                    dish.Visibility = true;
                _db.Entry(dish).State = EntityState.Modified;
            }

            await _db.SaveChangesAsync();
            return Json(true);
        }
    }
}