using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Starkit.Models;
using Starkit.Models.Data;
using Starkit.Services;

namespace Starkit.Controllers
{
    public class DishesController : Controller
    {
        private StarkitContext _db;
        private IHostEnvironment _environment;
        private UploadService _uploadService;

        public DishesController(StarkitContext db, IHostEnvironment environment, UploadService uploadService)
        {
            _db = db;
            _environment = environment;
            _uploadService = uploadService;
        }

        private string Load(string id, IFormFile file)
        {
            string path = Path.Combine(_environment.ContentRootPath + $"\\wwwroot\\images\\Dishes\\{id}");
            string photoPath = $"images/Dishes/{id}/{file.FileName}";
            if (!Directory.Exists($"wwwroot/images/Dishes/{id}"))
            {
                Directory.CreateDirectory($"wwwroot/images/Dishes/{id}");
            }
            _uploadService.Upload(path, file.FileName, file);
            return photoPath;
        }

        [HttpGet]
        public IActionResult Create()
        {
            Dish dish = new Dish{Categories = _db.Categories.ToList()};
            return View(dish);
        }
        
        [HttpPost]
        public async Task<IActionResult> Create(Dish dish)
        {
            if (ModelState.IsValid)
            {
                dish.Avatar = Load(dish.Id, dish.File);
                _db.Entry(dish).State = EntityState.Added;
                await _db.SaveChangesAsync();
            }
            return View(dish);
        }
    }
}