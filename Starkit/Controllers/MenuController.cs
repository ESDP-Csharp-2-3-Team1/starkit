using System;
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
    public class MenuController : Controller
    {
        private StarkitContext _db;
        private UserManager<User> _userManager { get; set; }
        private IHostEnvironment _environment;
        private UploadService _uploadService;

        public MenuController(StarkitContext db, IHostEnvironment environment, UploadService uploadService, UserManager<User> userManager)
        {
            _db = db;
            _environment = environment;
            _uploadService = uploadService;
            _userManager = userManager;
        }

        private string Load(string id, IFormFile file)
        {
            string userId = _userManager.GetUserId(User);
            string path = Path.Combine(_environment.ContentRootPath + $"\\wwwroot\\images\\{userId}\\Menu\\{id}");
            string photoPath = $"images/{userId}/Menu/{id}/{file.FileName}";
            if (!Directory.Exists($"wwwroot/images/{userId}/Menu/{id}"))
            {
                Directory.CreateDirectory($"wwwroot/images/{userId}/Menu/{id}");
            }
            _uploadService.Upload(path, file.FileName, file);
            return photoPath;
        }
        
        private void DeleteMenuAvatar(Menu menu)
        {
            string userId = _userManager.GetUserId(User);
            string filePath = _environment.ContentRootPath + $"\\wwwroot\\images\\{userId}\\Menu\\" + menu.Id; 
            if (Directory.Exists(filePath))
            {
                System.IO.File.Delete("wwwroot/" + menu.Avatar);
            }
        }

        [Authorize]
        public IActionResult ListMenu()
        {
            List<Menu> menu = _db.Menu.Where(d => d.CreatorId == _userManager.GetUserId(User)).ToList();
            return View(menu);
        }

        public IActionResult GetMenu()
        {
            List<Menu> menu = _db.Menu.Where(m => m.CreatorId == _userManager.GetUserId(User)).ToList();
            return PartialView("PartialViews/ListMenuPartialView", menu);
        }

        [Authorize]
        public IActionResult Create()
        {
            List<Dish> dishes = _db.Dishes.Where(d => d.CreatorId == _userManager.GetUserId(User)).ToList();
            var dishGroups = dishes.GroupBy(d => d.Category);
            Menu menu = new Menu{Dishes = dishGroups};
            return View(menu);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Menu menu)
        {
            if (ModelState.IsValid)
            {
                foreach (var dishId in menu.DishesId)
                {
                    MenuDish menuDish = new MenuDish{MenuId = menu.Id, DishId = dishId};
                    _db.Entry(menuDish).State = EntityState.Added;
                }
                menu.CreatorId = _userManager.GetUserId(User);
                menu.Avatar = Load(menu.Id, menu.File);
                menu.AddTime = DateTime.Now;
                _db.Entry(menu).State = EntityState.Added;
                await _db.SaveChangesAsync();
                return RedirectToAction("ListMenu");
            }
            List<Dish> dishes = _db.Dishes.Where(d => d.CreatorId == _userManager.GetUserId(User)).ToList();
            var dishGroups = dishes.GroupBy(d => d.Category);
            menu = new Menu{Dishes = dishGroups};
            return View(menu);
        }

        [HttpGet]
        public IActionResult Edit(string id)
        {
            List<Dish> dishes = _db.Dishes.Where(d => d.CreatorId == _userManager.GetUserId(User)).ToList();
            var dishGroups = dishes.GroupBy(d => d.Category);
            Menu menu = _db.Menu.FirstOrDefault(m => m.Id == id);
            EditMenuViewModel editMenuViewModel = new EditMenuViewModel
            {
                Id =  id,
                Name = menu.Name,
                Cost = menu.Cost,
                Dishes = dishGroups,
            };
            return View(editMenuViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditMenuViewModel model)
        {
            if (ModelState.IsValid)
            {
                Menu menu = _db.Menu.FirstOrDefault(m => m.Id == model.Id);
                menu.Name = model.Name;
                menu.Cost = model.Cost;
                menu.EditTime = DateTime.Now;
                menu.EditorId = _userManager.GetUserId(User);
                if (model.DishesId != null)
                {
                    List<MenuDish> menuDishes = _db.MenuDishes.Where(m => m.MenuId == model.Id).ToList();
                    _db.RemoveRange(menuDishes);
                    foreach (var dishId in model.DishesId)
                    {
                        MenuDish menuDish = new MenuDish{MenuId = menu.Id, DishId = dishId};
                        _db.Entry(menuDish).State = EntityState.Added;
                    }
                }
                if (model.File != null)
                {
                    DeleteMenuAvatar(menu);
                    menu.Avatar = Load(model.Id, model.File);
                }
                _db.Entry(menu).State = EntityState.Modified;
                await _db.SaveChangesAsync();
                return RedirectToAction("ListMenu");
            }
            return View(model);
        }
    }
}