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

        public IActionResult Index()
        {
            return View(_db.Menu.Where(m => m.CreatorId == _userManager.GetUserId(User)).ToList());
        }
        
        public IActionResult GetMenu()
        {
            List<Menu> menu = _db.Menu.Where(m => m.CreatorId == _userManager.GetUserId(User)).ToList();
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
                menu.Avatar = menu.Avatar = Load(menu.Id, menu.File);
                menu.AddTime = DateTime.Now;
                menu.CreatorId = _userManager.GetUserId(User);
                _db.Entry(menu).State = EntityState.Added;
                await _db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(menu);
        }
        
        [HttpDelete]
        public async Task<IActionResult> Delete(string id)
        {
            Menu menu = new Menu{Id = id};
            _db.Entry(menu).State = EntityState.Deleted;
            await _db.SaveChangesAsync();
            List<Menu> listMenu = _db.Menu.Where(c => c.CreatorId == _userManager.GetUserId(User)).ToList();
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
                var menu = _db.Menu.FirstOrDefault(m => m.Id == model.Id);
                menu.Name = model.Name;
                menu.Type = model.Type;
                menu.Cost = model.Cost;
                menu.EditTime = DateTime.Now;
                menu.EditorId = _userManager.GetUserId(User);
                menu.Description = model.Description;
                if (model.File != null)
                {
                    DeleteMenuAvatar(menu);
                    menu.Avatar = Load(model.Id, model.File);
                }
                
                _db.Entry(menu).State = EntityState.Modified;
                await _db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View();
        }
    }
}