using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Starkit.Models;
using Starkit.Models.Data;

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
                _db.Entry(subCategory).State = EntityState.Added;
                await _db.SaveChangesAsync();
                return RedirectToAction("Create");
            }
            return View(subCategory);
        }
    }
}