using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Starkit.Models;
using Starkit.Models.Data;

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
        [HttpGet]
        public IActionResult Create()
        {
            return View(new Category());
        }
        
        [HttpPost]
        public async Task<IActionResult> Create(Category category)
        {
            if (ModelState.IsValid)
            {
                category.UserId = _userManager.GetUserId(User);
                _db.Entry(category).State = EntityState.Added;
                await _db.SaveChangesAsync();
                return RedirectToAction("Create");
            }
            return View(category);
        }
    }
}