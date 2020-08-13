using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Starkit.Models;
using Starkit.Models.Data;
using Starkit.ViewModels;

namespace Starkit.Controllers
{
    public class RestaurantsController : Controller
    {
        private StarkitContext _db { get; set; }
        private UserManager<User> _userManager { get; set; }
        private IHostEnvironment _environment { get; set; }

        public RestaurantsController(StarkitContext db, UserManager<User> userManager, IHostEnvironment environment)
        {
            _db = db;
            _userManager = userManager;
            _environment = environment;
        }


        // GET
        [Authorize]
        public IActionResult Register()
        {
            return View();
        }
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Register(Restaurant model)
        {
            if (ModelState.IsValid)
            {
                string userId = _userManager.GetUserId(User);
                model.UserId = userId;
                await _db.Restaurants.AddAsync(model);
                await _db.SaveChangesAsync();
                return RedirectToAction("Index", "Users");
            }
            return View(model);
        }
    }
}