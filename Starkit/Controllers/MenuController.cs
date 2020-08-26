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

        private async Task<string> Load(string id, IFormFile file)
        {
            string userId = _userManager.GetUserId(User);
            if (User.IsInRole(Convert.ToString(Roles.SuperAdmin)))
            {
                User admin = await _db.Users.FirstOrDefaultAsync(u => u.Id == userId);
                userId = admin.IdOfTheSelectedRestaurateur;
            }
            string path = Path.Combine(_environment.ContentRootPath + $"\\wwwroot\\images\\users\\{userId}\\Menu\\{id}");
            string photoPath = $"images/users/{userId}/Menu/{id}/{file.FileName}";
            if (!Directory.Exists($"wwwroot/images/users/{userId}/Menu/{id}"))
            {
                Directory.CreateDirectory($"wwwroot/images/users/{userId}/Menu/{id}");
            }
            await _uploadService.Upload(path, file.FileName, file);
            return photoPath;
        }
        
        private async void DeleteMenuAvatar(Menu menu)
        {
            string userId = _userManager.GetUserId(User);
            if (User.IsInRole(Convert.ToString(Roles.SuperAdmin)))
            {
                User admin = await _db.Users.FirstOrDefaultAsync(u => u.Id == userId);
                userId = admin.IdOfTheSelectedRestaurateur;
            }
            string filePath = _environment.ContentRootPath + $"\\wwwroot\\images\\users\\{userId}\\Menu\\" + menu.Id; 
            if (Directory.Exists(filePath))
            {
                if (menu.Avatar == null)
                    Directory.Delete(filePath, true);
                else
                    System.IO.File.Delete("wwwroot/" + menu.Avatar);
            }
        }

        public async Task<IActionResult> Index()
        {
            string userId = _userManager.GetUserId(User);
            if (User.IsInRole(Convert.ToString(Roles.SuperAdmin)))
            {
                User admin = await _db.Users.FirstOrDefaultAsync(u => u.Id == userId);
                userId = admin.IdOfTheSelectedRestaurateur;
            }
            return View(_db.Menu.Where(m => m.CreatorId == userId).ToList());
        }
        
        public async Task<IActionResult> GetMenu()
        {
            string userId = _userManager.GetUserId(User);
            if (User.IsInRole(Convert.ToString(Roles.SuperAdmin)))
            {
                User admin = await _db.Users.FirstOrDefaultAsync(u => u.Id == userId);
                userId = admin.IdOfTheSelectedRestaurateur;
            }
            List<Menu> menu = _db.Menu.Where(m => m.CreatorId == userId).ToList();
            return PartialView("PartialViews/ListMenuPartialView", menu);
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View();
        }
        
        [HttpPost]
        public async Task<IActionResult> Create(Menu menu)
        {
            if (ModelState.IsValid)
            {
                string userId = _userManager.GetUserId(User);
                if (User.IsInRole(Convert.ToString(Roles.SuperAdmin)))
                {
                    User admin = await _db.Users.FirstOrDefaultAsync(u => u.Id == userId);
                    userId = admin.IdOfTheSelectedRestaurateur;
                }
                menu.Avatar = menu.Avatar = await Load(menu.Id, menu.File);
                menu.AddTime = DateTime.Now;
                menu.CreatorId = userId;
                _db.Entry(menu).State = EntityState.Added;
                await _db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(menu);
        }
        
        [HttpDelete]
        public async Task<IActionResult> Delete(string id)
        {
            string userId = _userManager.GetUserId(User);
            if (User.IsInRole(Convert.ToString(Roles.SuperAdmin)))
            {
                User admin = await _db.Users.FirstOrDefaultAsync(u => u.Id == userId);
                userId = admin.IdOfTheSelectedRestaurateur;
            }
            Menu menu = new Menu{Id = id};
            _db.Entry(menu).State = EntityState.Deleted;
            await _db.SaveChangesAsync();
            DeleteMenuAvatar(menu);
            List<Menu> listMenu = _db.Menu.Where(c => c.CreatorId == userId).ToList();
            return PartialView("PartialViews/ListMenuPartialView", listMenu);
        }

        [HttpGet]
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

        [HttpPost]
        public async Task<IActionResult> Edit(EditMenuViewModel model)
        {
            if (ModelState.IsValid)
            {
                string userId = _userManager.GetUserId(User);
                if (User.IsInRole(Convert.ToString(Roles.SuperAdmin)))
                {
                    User admin = await _db.Users.FirstOrDefaultAsync(u => u.Id == userId);
                    userId = admin.IdOfTheSelectedRestaurateur;
                }
                var menu = _db.Menu.FirstOrDefault(m => m.Id == model.Id);
                menu.Name = model.Name;
                menu.Type = model.Type;
                menu.Cost = model.Cost;
                menu.EditTime = DateTime.Now;
                menu.EditorId = userId;
                menu.Description = model.Description;
                if (model.File != null)
                {
                    DeleteMenuAvatar(menu);
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