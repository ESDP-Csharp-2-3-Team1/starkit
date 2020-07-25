using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Starkit.Models;
using Starkit.Models.Data;

namespace Starkit.Controllers
{
    public class CategoriesController : Controller
    {
        private StarkitContext _db;

        public CategoriesController(StarkitContext db)
        {
            _db = db;
        }
        
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
                _db.Entry(category).State = EntityState.Added;
                await _db.SaveChangesAsync();
                return RedirectToAction("Create");
            }
            return View(category);
        }
    }
}