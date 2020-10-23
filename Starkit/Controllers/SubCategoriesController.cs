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
    [Authorize(Roles = "SuperAdmin,Registrant,ContentManager")]
    public class SubCategoriesController : Controller
    {
        private StarkitContext _db;
        private UserManager<User> _userManager { get; set; }

        public SubCategoriesController(StarkitContext db, UserManager<User> userManager)
        {
            _db = db;
            _userManager = userManager;
        }

        [Authorize]
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
            SubCategory subCategory = new SubCategory{Categories = _db.Categories.
                Where(c => c.RestaurantId == user.RestaurantId).ToList()};
            return View(subCategory);
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create(SubCategory subCategory)
        {
            if (ModelState.IsValid)
            {
                User user = await _userManager.FindByIdAsync(_userManager.GetUserId(User));
                if (User.IsInRole("SuperAdmin"))
                {
                    user = await _userManager.FindByIdAsync(user.IdOfTheSelectedRestaurateur);
                    subCategory.UserId = user.Id;
                    subCategory.RestaurantId = user.RestaurantId;
                }
                else
                {
                    subCategory.UserId = user.Id;
                    subCategory.RestaurantId = user.RestaurantId;
                }
                subCategory.CreateTime = DateTime.Now;
                _db.Entry(subCategory).State = EntityState.Added;
                await _db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(subCategory);
        }
        
        [HttpDelete]
        [Authorize]
        public async Task<IActionResult> Delete(string id)
        {
            SubCategory subCategory = new SubCategory{Id = id};
            _db.Entry(subCategory).State = EntityState.Deleted;
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [HttpGet]
        [Authorize]
        public IActionResult Edit(string id)
        {
            SubCategory subCategory = _db.SubCategories.FirstOrDefault(c => c.Id == id);
            EditSubCategoryViewModel model = new EditSubCategoryViewModel{Id = id, Name = subCategory.Name, Category = subCategory.Category};
            return View(model);
        }
        
        [ValidateAntiForgeryToken]
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Edit(EditSubCategoryViewModel model)
        {
            if (ModelState.IsValid)
            {
                SubCategory subCategory = _db.SubCategories.FirstOrDefault(c => c.Id == model.Id);
                if (model.Name != subCategory.Name)
                    subCategory.EditedTime = DateTime.Now;
                subCategory.Name = model.Name;
                _db.Entry(subCategory).State = EntityState.Modified;
                await _db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(model);
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
        public async Task<IActionResult> GetSubCategories()
        {
            User user = await _userManager.FindByIdAsync(_userManager.GetUserId(User));
            if (User.IsInRole("SuperAdmin"))
            {
                string userId = user.IdOfTheSelectedRestaurateur;
                user = await _userManager.FindByIdAsync(userId);
            }
            Restaurant restaurant = await _db.Restaurants.
                FirstOrDefaultAsync(r => r.Id == user.RestaurantId);
            return PartialView("PartialViews/ListSubCategoryPartialView", restaurant.SubCategories);
        }
    }
}