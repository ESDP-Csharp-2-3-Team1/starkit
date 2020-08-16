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
        public IActionResult Create()
        {
            SubCategory subCategory = new SubCategory{Categories = _db.Categories.Where(c => 
                c.UserId == _userManager.GetUserId(User)).ToList()};
            return View(subCategory);
        }

        [HttpPost]
        public async Task<IActionResult> Create(SubCategory subCategory)
        {
            if (ModelState.IsValid)
            {
                subCategory.CreateTime = DateTime.Now;
                subCategory.UserId = _userManager.GetUserId(User);
                _db.Entry(subCategory).State = EntityState.Added;
                await _db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(subCategory);
        }
        
        [HttpDelete]
        public async Task<IActionResult> Delete(string id)
        {
            SubCategory subCategory = new SubCategory{Id = id};
            _db.Entry(subCategory).State = EntityState.Deleted;
            await _db.SaveChangesAsync();
            List<SubCategory> subCategories = _db.SubCategories.Where(c => c.UserId == _userManager.GetUserId(User)).ToList();
            return PartialView("PartialViews/ListSubCategoryPartialView", subCategories);
        }

        [HttpGet]
        [Authorize]
        public IActionResult Edit(string id)
        {
            SubCategory subCategory = _db.SubCategories.FirstOrDefault(c => c.Id == id);
            EditSubCategoryViewModel model = new EditSubCategoryViewModel{Id = id, Name = subCategory.Name};
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditSubCategoryViewModel model)
        {
            if (ModelState.IsValid)
            {
                SubCategory subCategory = _db.SubCategories.FirstOrDefault(c => c.Id == model.Id);
                if (model.Name != subCategory.Name)
                    subCategory.EditedTime = DateTime.Now;
                subCategory.Name = model.Name;
                subCategory.UserId = _userManager.GetUserId(User);
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

        public IActionResult GetSubCategories()
        {
            List<SubCategory> subCategories =_db.SubCategories.Where(c => c.UserId == _userManager.GetUserId(User)).ToList();
            return PartialView("PartialViews/ListSubCategoryPartialView", subCategories);
        }
    }
}