using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Starkit.Models;
using Starkit.Models.Data;
using Starkit.Services;
using Starkit.ViewModels;

namespace Starkit.Controllers
{
    public class CategoriesController : Controller
    {
        private StarkitContext _db;
        private UserManager<User> _userManager { get; set; }

        public CategoriesController(StarkitContext db, UserManager<User> userManager)
        {
            _db = db;
            _userManager = userManager;
        }
        
        [Authorize]
        public IActionResult Index()
        {
            return View();
        }

        [Authorize]
        public async Task<IActionResult> GetCategories()
        {
            string userId = _userManager.GetUserId(User);
            if (User.IsInRole("SuperAdmin"))
            {
                User user = await _userManager.FindByIdAsync(userId);
                userId = user.IdOfTheSelectedRestaurateur;
            }
            List<Category> categories =_db.Categories.Where(c => c.UserId == userId).ToList();
            return PartialView("PartialViews/ListCategoryPartialView", categories);
        }

        [Authorize]
        [HttpGet]
        public IActionResult Create()
        {
            return View(new Category());
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create(Category category)
        {
            if (ModelState.IsValid)
            {
                string userId = _userManager.GetUserId(User);
                if (User.IsInRole(Convert.ToString(Roles.SuperAdmin)))
                {
                    User admin = await _userManager.FindByIdAsync(userId);
                    category.UserId = admin.IdOfTheSelectedRestaurateur;
                }
                else
                    category.UserId = userId;
                
                category.CreateTime = DateTime.Now;
                _db.Entry(category).State = EntityState.Added;
                await _db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(category);
        }

        [Authorize]
        [HttpDelete]
        public async Task<IActionResult> Delete(string id)
        {
            string userId = _userManager.GetUserId(User);
            if (User.IsInRole(Convert.ToString(Roles.SuperAdmin)))
            {
                User admin = await _db.Users.FirstOrDefaultAsync(u => u.Id == userId);
                userId = admin.IdOfTheSelectedRestaurateur;
            }
            Category category = new Category{Id = id};
            _db.Entry(category).State = EntityState.Deleted;
            await _db.SaveChangesAsync();
            List<Category> categories = _db.Categories.Where(c => c.UserId == userId).ToList();
            return PartialView("PartialViews/ListCategoryPartialView", categories);
        }

        [HttpGet]
        [Authorize]
        public IActionResult Edit(string id)
        {
            Category category = _db.Categories.FirstOrDefault(c => c.Id == id);
            EditCategoryViewModel model = new EditCategoryViewModel{Id = id, Name = category.Name};
            return View(model);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Edit(EditCategoryViewModel model)
        {
            if (ModelState.IsValid)
            {
                Category category = _db.Categories.FirstOrDefault(c => c.Id == model.Id);
                if (model.Name != category.Name)
                    category.EditedTime = DateTime.Now;
                category.Name = model.Name;
                _db.Entry(category).State = EntityState.Modified;
                await _db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(model);
        }
    }
}