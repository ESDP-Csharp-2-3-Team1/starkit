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

        public IActionResult Index()
        {
            return View();
        }

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
                return RedirectToAction("Create");
            }
            List<Dish> dishes = _db.Dishes.Where(d => d.CreatorId == _userManager.GetUserId(User)).ToList();
            var dishGroups = dishes.GroupBy(d => d.Category);
            menu = new Menu{Dishes = dishGroups};
            return View(menu);
        }
    }
}