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
        public async Task<IActionResult> GetCategories()
        {
            User user = await _userManager.FindByIdAsync(_userManager.GetUserId(User));
            if (User.IsInRole("SuperAdmin"))
            {
                string userId = user.IdOfTheSelectedRestaurateur;
                user = await _userManager.FindByIdAsync(userId);
            }
            Restaurant restaurant = await _db.Restaurants.
                FirstOrDefaultAsync(r => r.Id == user.RestaurantId);
            return PartialView("PartialViews/ListCategoryPartialView", restaurant.Categories);
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
            return View(new Category());
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create(Category category)
        {
            if (ModelState.IsValid)
            {
                User user = await _userManager.FindByIdAsync(_userManager.GetUserId(User));
                if (User.IsInRole("SuperAdmin"))
                {
                    user = await _userManager.FindByIdAsync(user.IdOfTheSelectedRestaurateur);
                    category.UserId = user.Id;
                    category.RestaurantId = user.RestaurantId;
                }
                else
                {
                    category.UserId = user.Id;
                    category.RestaurantId = user.RestaurantId;
                }
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
            Category category = new Category{Id = id};
            _db.Entry(category).State = EntityState.Deleted;
            await _db.SaveChangesAsync();
            return RedirectToAction("GetCategories");
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