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
        private UserManager<User> _userManager;
        private IHostEnvironment _environment;
        private UploadService _uploadService;

        public MenuController(StarkitContext db, IHostEnvironment environment, UploadService uploadService, UserManager<User> userManager)
        {
            _db = db;
            _environment = environment;
            _uploadService = uploadService;
            _userManager = userManager;
        }

        [Authorize(Roles = "SuperAdmin,Registrant,ContentManager")]
        private async Task<string> Load(string id, IFormFile file)
        {
            User user = await _userManager.FindByIdAsync(_userManager.GetUserId(User));
            if (User.IsInRole(Convert.ToString(Roles.SuperAdmin)))
            {
                string userId = user.IdOfTheSelectedRestaurateur;
                user = await _userManager.FindByIdAsync(userId);
            }
            string path = Path.Combine(_environment.ContentRootPath + $"/wwwroot/images/restaurants/{user.RestaurantId}/Menu/{id}");
            string photoPath = $"images/restaurants/{user.RestaurantId}/Menu/{id}/{file.FileName}";
            if (!Directory.Exists($"wwwroot/images/restaurants/{user.RestaurantId}/Menu/{id}"))
            {
                Directory.CreateDirectory($"wwwroot/images/restaurants/{user.RestaurantId}/Menu/{id}");
            }
            await _uploadService.Upload(path, file.FileName, file);
            return photoPath;
        }
        
        [Authorize(Roles = "SuperAdmin,Registrant,ContentManager")]
        private async Task DeleteMenuAvatar(Menu menu)
        {
            User user = await _userManager.FindByIdAsync(_userManager.GetUserId(User));
            if (User.IsInRole(Convert.ToString(Roles.SuperAdmin)))
            {
                string userId = user.IdOfTheSelectedRestaurateur;
                user = await _userManager.FindByIdAsync(userId);
            }
            string filePath = _environment.ContentRootPath + $"/wwwroot/images/restaurants/{user.RestaurantId}/Menu/" + menu.Id; 
            if (Directory.Exists(filePath))
            {
                if (menu.Avatar == null)
                    Directory.Delete(filePath, true);
                else
                    System.IO.File.Delete("wwwroot/" + menu.Avatar);
            }
        }

        [Authorize(Roles = "SuperAdmin,Registrant,ContentManager")]
        public async Task<IActionResult> Index()
        {
            User user = await _userManager.FindByIdAsync(_userManager.GetUserId(User));
            if (User.IsInRole(Convert.ToString(Roles.SuperAdmin)))
            {
                string userId = user.IdOfTheSelectedRestaurateur;
                user = await _userManager.FindByIdAsync(userId);
            }
            if (user.RestaurantId == null)
                return RedirectToAction("Register", "Restaurants");
            return View();
        }
        
        [Authorize(Roles = "SuperAdmin,Registrant,ContentManager")]
        public async Task<IActionResult> GetMenu()
        {
            User user = await _userManager.FindByIdAsync(_userManager.GetUserId(User));
            if (User.IsInRole(Convert.ToString(Roles.SuperAdmin)))
            {
                string userId = user.IdOfTheSelectedRestaurateur;
                user = await _userManager.FindByIdAsync(userId);
            }
            List<Menu> menu = _db.Menu.Where(m => m.RestaurantId == user.RestaurantId).ToList();
            return PartialView("PartialViews/ListMenuPartialView", menu);
        }

        [Authorize(Roles = "SuperAdmin,Registrant")]

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            User user = await _userManager.FindByIdAsync(_userManager.GetUserId(User));
            if (User.IsInRole(Convert.ToString(Roles.SuperAdmin)))
            {
                string userId = user.IdOfTheSelectedRestaurateur;
                user = await _userManager.FindByIdAsync(userId);
            }
            if (user.RestaurantId == null)
                return RedirectToAction("Register", "Restaurants");
            return View();
        }
        
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "SuperAdmin,Registrant")]
        [HttpPost]
        public async Task<IActionResult> Create(Menu menu)
        {
            if (ModelState.IsValid)
            {
                User user = await _userManager.FindByIdAsync(_userManager.GetUserId(User));
                if (User.IsInRole(Convert.ToString(Roles.SuperAdmin)))
                    user = await _userManager.FindByIdAsync(user.IdOfTheSelectedRestaurateur);
                menu.Avatar = menu.Avatar = await Load(menu.Id, menu.File);
                menu.AddTime = DateTime.Now;
                menu.CreatorId = user.Id;
                menu.RestaurantId = user.RestaurantId;
                _db.Entry(menu).State = EntityState.Added;
                await _db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(menu);
        }
        
        [Authorize(Roles = "SuperAdmin,Registrant")]
        [HttpDelete]
        public async Task<IActionResult> Delete(string id)
        {
            Menu menu = new Menu{Id = id};
            _db.Entry(menu).State = EntityState.Deleted;
            await _db.SaveChangesAsync();
            await DeleteMenuAvatar(menu);
            return RedirectToAction("GetMenu");
        }

        [HttpGet]
        [Authorize(Roles = "SuperAdmin,Registrant")]
        public IActionResult Edit(string id)
        {
            Menu menu = _db.Menu.FirstOrDefault(m => m.Id == id);
            EditMenuViewModel model = new EditMenuViewModel
            {
                Id = menu.Id,
                Name = menu.Name,
                Type = menu.Type,
                Cost = menu.Cost,
                Description = menu.Description,
            };
            return View(model);
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        [Authorize(Roles = "SuperAdmin,Registrant")]
        public async Task<IActionResult> Edit(EditMenuViewModel model)
        {
            if (ModelState.IsValid)
            {
                User user = await _userManager.FindByIdAsync(_userManager.GetUserId(User));
                if (User.IsInRole(Convert.ToString(Roles.SuperAdmin)))
                    user = await _userManager.FindByIdAsync(user.IdOfTheSelectedRestaurateur);
                var menu = _db.Menu.FirstOrDefault(m => m.Id == model.Id);
                menu.Name = model.Name;
                menu.Type = model.Type;
                menu.Cost = model.Cost;
                menu.EditTime = DateTime.Now;
                menu.EditorId = user.Id;
                menu.Description = model.Description;
                if (model.File != null)
                {
                    await DeleteMenuAvatar(menu);
                    menu.Avatar = await Load(model.Id, model.File);
                }
                
                _db.Entry(menu).State = EntityState.Modified;
                await _db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View();
        }
        
        [HttpGet]
        public IActionResult Details(string id)
        {
            return PartialView("PartialViews/DetailMenuModalWindowPartialView",
                _db.Menu.FirstOrDefault(m => m.Id == id));
        }
    }
}