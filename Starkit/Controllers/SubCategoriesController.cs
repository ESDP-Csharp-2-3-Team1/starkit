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
            string userId = _userManager.GetUserId(User);
            if (User.IsInRole(Convert.ToString(Roles.SuperAdmin)))
            {
                User admin = await _db.Users.FirstOrDefaultAsync(u => u.Id == userId);
                userId = admin.IdOfTheSelectedRestaurateur;
            }
            SubCategory subCategory = new SubCategory{Categories = _db.Categories.Where(c => 
                c.UserId == userId).ToList()};
            return View(subCategory);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create(SubCategory subCategory)
        {
            if (ModelState.IsValid)
            {
                string userId = _userManager.GetUserId(User);
                if (User.IsInRole(Convert.ToString(Roles.SuperAdmin)))
                {
                    User admin = await _db.Users.FirstOrDefaultAsync(u => u.Id == userId);
                    userId = admin.IdOfTheSelectedRestaurateur;
                }
                subCategory.CreateTime = DateTime.Now;
                subCategory.UserId = userId;
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
            string userId = _userManager.GetUserId(User);
            if (User.IsInRole(Convert.ToString(Roles.SuperAdmin)))
            {
                User admin = await _db.Users.FirstOrDefaultAsync(u => u.Id == userId);
                userId = admin.IdOfTheSelectedRestaurateur;
            }
            SubCategory subCategory = new SubCategory{Id = id};
            _db.Entry(subCategory).State = EntityState.Deleted;
            await _db.SaveChangesAsync();
            List<SubCategory> subCategories = _db.SubCategories.Where(c => c.UserId == userId).ToList();
            return PartialView("PartialViews/ListSubCategoryPartialView", subCategories);
        }

        [HttpGet]
        [Authorize]
        public IActionResult Edit(string id)
        {
            SubCategory subCategory = _db.SubCategories.FirstOrDefault(c => c.Id == id);
            EditSubCategoryViewModel model = new EditSubCategoryViewModel{Id = id, Name = subCategory.Name, Category = subCategory.Category};
            return View(model);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Edit(EditSubCategoryViewModel model)
        {
            if (ModelState.IsValid)
            {
                string userId = _userManager.GetUserId(User);
                if (User.IsInRole(Convert.ToString(Roles.SuperAdmin)))
                {
                    User admin = await _db.Users.FirstOrDefaultAsync(u => u.Id == userId);
                    userId = admin.IdOfTheSelectedRestaurateur;
                }
                SubCategory subCategory = _db.SubCategories.FirstOrDefault(c => c.Id == model.Id);
                if (model.Name != subCategory.Name)
                    subCategory.EditedTime = DateTime.Now;
                subCategory.Name = model.Name;
                subCategory.UserId = userId;
                _db.Entry(subCategory).State = EntityState.Modified;
                await _db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(model);
        }

        [Authorize]
        public IActionResult Index()
        {
            return View();
        }

        [Authorize]
        public async Task<IActionResult> GetSubCategories()
        {
            string userId = _userManager.GetUserId(User);
            if (User.IsInRole(Convert.ToString(Roles.SuperAdmin)))
            {
                User admin = await _db.Users.FirstOrDefaultAsync(u => u.Id == userId);
                userId = admin.IdOfTheSelectedRestaurateur;
            }
            List<SubCategory> subCategories =_db.SubCategories.Where(c => c.UserId == userId).ToList();
            return PartialView("PartialViews/ListSubCategoryPartialView", subCategories);
        }
    }
}